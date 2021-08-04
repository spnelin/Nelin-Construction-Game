import React, { Component } from 'react';
import { OpportunityInfo, ExecutiveAssignment } from './ExecutiveAssignment';
import { Modal, ModalSubmitCancel } from './Modal';
import { CardSelection } from './CardSelection';
import Utility from './Utility';

export class CardHand extends Component {
    constructor(props) {
        super(props);

        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;

        this.state = {
            ui: {
                //highlighting?
            },
            data: this.publicFunctions.getServerObject(this.constants.OBJECT_HAND)
        };

        this.updateHand = this.updateHand.bind(this);
        this.renderCard = this.renderCard.bind(this);
        this.submit = this.submit.bind(this);
        this.cancel = this.cancel.bind(this);
        this.disableSubmit = this.disableSubmit.bind(this);

        this.publicFunctions.addListener(this.constants.OBJECT_HAND, this.updateHand);
    }

    updateHand(hand) {
        this.setState({ data: hand });
    }

    submit(card, index) {
        this.publicFunctions.post("PlayCard", { cardIndex: index });
    }

    cancel() {
        this.publicFunctions.closeModal();
    }

    disableSubmit(card) {
        return false; //eventually some cards might have costs to play?
    }

    renderCard(card) {
        if (card.cardType === this.enums.CardType.Opportunity) {
            return <OpportunityInfo opportunity={card} enums={this.enums} />
        }
        else {
            return ""; //intrigue will go here
        }
    }

    render() {
        return (
            <Modal>
                <CardSelection title="Your Cards"
                    cards={this.state.data.cards}
                    cardRenderer={this.renderCard}
                    selectionHandler={this.submit}
                    selectionDisabled={this.disableSubmit}
                    selectionText="Play"
                    cancelHandler={this.cancel}
                    cancelText="Cancel" />
            </Modal>
        );
    }
}

export class CardHandMinimized extends Component {
    constructor(props) {
        super(props);

        console.log("Public Objects")
        console.log(props.publicObjects)

        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;

        this.state = {
            ui: {
                //highlighting?
            },
            data: this.publicFunctions.getServerObject(this.constants.OBJECT_HAND)
        };

        this.openHand = this.openHand.bind(this);
        this.handDisabled = this.handDisabled.bind(this);
        this.updateHand = this.updateHand.bind(this);
        this.renderCardHand = this.renderCardHand.bind(this);

        this.getTaskIndex = this.getTaskIndex.bind(this);
        this.launchExecutiveAssignmentModal = this.launchExecutiveAssignmentModal.bind(this);
        this.renderExecutiveAssignmentModal = this.renderExecutiveAssignmentModal.bind(this);

        this.publicFunctions.addListener(this.constants.OBJECT_HAND, this.updateHand);
    }

    updateHand(hand) {
        this.setState({ data: hand });
    }

    openHand(e) {
        e.preventDefault();
        this.publicFunctions.launchCustomModal(this.renderCardHand);
    }

    handDisabled() {
        return this.state.data.cards.length === 0;
    }

    renderCardHand() {
        return <CardHand publicObjects={this.publicFunctions.getPublicObjects()} />;
    }

    getTaskIndex(type) {
        let taskList = this.publicFunctions.getServerObject(this.constants.OBJECT_EXEC_INFO).taskList
        for (let index = 0; index < taskList.length; index++) {
            if (taskList[index].type === type) {
                return index;
            }
        }
        return -1;
    }

    launchExecutiveAssignmentModal(taskIndex) {
        this.publicFunctions.launchCustomModal(() => this.renderExecutiveAssignmentModal(taskIndex));
    }

    renderExecutiveAssignmentModal(taskIndex) {
        return (<ExecutiveAssignment execIndex={null} taskIndex={taskIndex} publicObjects={this.publicFunctions.getPublicObjects()} />);
    }

    render() {
        let taskType = this.getTaskIndex(this.enums.ExecutiveTaskType.FindOpportunity);
        return (
            <div>
                <button onClick={this.openHand} disabled={this.handDisabled()}>View Opportunities</button>({this.state.data.cards.length} cards)<br />
                <button onClick={() => this.launchExecutiveAssignmentModal(taskType)}>Find Opportunity</button>
            </div>
        );
    }
}

export class MaterialDealOpportunity extends Component {
    constructor(props) {
        super(props);

        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;
        this.settings = props.publicObjects.settings;

        let modal = this.publicFunctions.getServerObject(this.constants.OBJECT_MODAL);
        
        this.money = this.publicFunctions.getServerObject(this.constants.OBJECT_CURRENT_PLAYER).money;

        this.state = {
            data: modal,
            ui: {
                quantity: 0,
                cost: 0,
                warnText: "",
                warning: false
            }
        };

        this.resTypes = []
        for (const prop in this.enums.MaterialType) {
            this.resTypes[this.enums.MaterialType[prop]] = prop
        }

        this.submit = this.submit.bind(this);
        this.onChangeQuant = this.onChangeQuant.bind(this);
        this.onDecQuant = this.onDecQuant.bind(this);
        this.onIncQuant = this.onIncQuant.bind(this);

        this.publicFunctions.addModalSubmitHandler(this.submit);
    }

    submit() {
        this.publicFunctions.post(`BuyMaterialDeal`, { quantity: this.state.ui.quantity });
    }

    onChangeQuant(e) {
        let quant = Number(e.target.value);
        if (isNaN(quant)) {
            return;
        }
        this.updateToQuant(quant);
    }

    onDecQuant(e) {
        e.preventDefault();
        let quant = this.state.ui.quantity;
        quant--;
        this.updateToQuant(quant);
    }

    onIncQuant(e) {
        e.preventDefault();
        let quant = this.state.ui.quantity;
        quant++;
        this.updateToQuant(quant);
    }

    updateToQuant(quant) {
        if (quant < 0) {
            quant = 0;
        }
        let warning = "";
        if (quant > this.state.data.maxQuantity) {
            quant = this.state.data.maxQuantity;
            warning = "You entered more than the maximum number you can buy.";
        }
        let costPer = this.state.data.price;
        let cost = quant * costPer;
        if (cost > this.money) {
            quant = Math.floor(this.money / costPer);
            warning = "You entered a number that would cost more than you can afford.";
        }
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                quantity: quant,
                cost: cost,
                warning: warning
            }
        }));
    }

    renderWarningText() {
        return (
            <div>
                {this.state.ui.warning}
            </div>
        );
    }

    submitDisabled() {
        return this.state.ui.quantity == 0;
    }

    renderMaterialInfo() {
        return (
            <div>
                Buying {this.resTypes[this.state.data.material]} (max {this.state.data.maxQuantity})<br />
                Your current funds: {Utility.moneyString(this.money)}<br />
            </div>
        );
    }

    renderMaterialQuant() {
        return (
            <div>
                <button onClick={this.onDecQuant}>-</button>
                <input type="text" pattern="[0-9]*"
                    value={this.state.ui.quantity}
                    onChange={this.onChangeQuant}
                    onKeyDown={this.onQuantKeyDown} />
                <button onClick={this.onIncQuant}>+</button>
                <br />Cost: {Utility.moneyString(this.state.ui.cost)}
            </div>
        );
    }

    render() {
        return (
            <Modal>
                {this.renderWarningText()}
                {this.renderMaterialInfo()}
                {this.renderMaterialQuant()}
                <ModalSubmitCancel onSubmit={this.publicFunctions.submitModal} onCancel={this.publicFunctions.closeModal} submitDisabled={this.submitDisabled()} />
            </Modal>
        );
    }
}