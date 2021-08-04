import React, { Component } from 'react';
import { CardSelection } from './CardSelection';
import { Modal, ModalSubmitCancel } from './Modal';
import Utility from './Utility';
import { ProjectStaticDisplay } from './Project';
import { ExecutiveInfo } from './Executive';

export class ExecutiveAssignment extends Component {
    constructor(props) {
        super(props);

        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;

        let canSelectExec = true;
        let canSelectTask = true;

        let execIndex = 0;
        let taskIndex = 0;

        if (Number.isInteger(props.execIndex)) {
            execIndex = props.execIndex;
            canSelectExec = false;
        }
        else {
            execIndex = 0;
            canSelectExec = true;
        }

        if (Number.isInteger(props.taskIndex)) {
            taskIndex = props.taskIndex;
            canSelectTask = false;
        }
        else {
            taskIndex = 0;
            canSelectTask = false;
        }

        this.state = {
            data: this.publicFunctions.getServerObject(this.constants.OBJECT_EXEC_INFO),
            ui: {
                execIndex: execIndex,
                taskIndex: taskIndex,
                canSelectExec: canSelectExec,
                canSelectTask: canSelectTask
            }
        };

        this.updateExecInfo = this.updateExecInfo.bind(this);
        this.onChangeExecutive = this.onChangeExecutive.bind(this);
        this.submit = this.submit.bind(this);

        this.renderBuyMaterialsModal = this.renderBuyMaterialsModal.bind(this);
        this.renderHireWorkModal = this.renderHireWorkModal.bind(this);

        this.publicFunctions.addListener(this.constants.OBJECT_EXEC_INFO, this.updateExecInfo);

        this.publicFunctions.addModalSubmitHandler(this.submit);
    }

    componentWillUnmount() {
        this.publicFunctions.removeListener(this.constants.OBJECT_EXEC_INFO, this.updateExecInfo);
    }

    updateExecInfo(data) {
        this.setState({ data: data });
    }

    onChangeExecutive(e) {
        let newIndex = e.target.value;
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                execIndex: newIndex
            }
        }));
    }

    onChangeTask(e) {
        let newIndex = e.target.value;
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                taskIndex: newIndex
            }
        }));
    }

    submit() {
        let task = this.state.data.taskList[this.state.ui.taskIndex].type;
        this.publicFunctions.post("AssignExecutive", { executiveIndex: this.state.ui.execIndex, task: task });
        switch (task) {
            case this.enums.ExecutiveTaskType.BuyMaterials:
                this.publicFunctions.launchCustomModal(this.renderBuyMaterialsModal);
                break;
            case this.enums.ExecutiveTaskType.HireWorkers:
                this.publicFunctions.launchCustomModal(this.renderHireWorkModal);
                break;
        }
    }

    renderBuyMaterialsModal(siteCore) {
        return (
            <div>
                <BuyMaterials publicObjects={siteCore.publicObjects} />
            </div>
        );
    }

    renderHireWorkModal(siteCore) {
        return (
            <div>
                <HireWorkers publicObjects={siteCore.publicObjects} />
            </div>
        );
    }

    renderExecutiveSelector() {
        return this.state.ui.canSelectExec ? (
            <div>
                <select value={this.state.ui.execIndex} onChange={this.onChangeExecutive}>
                    {this.state.data.executives.map((exec, index) =>
                        <option key={index} value={index} disabled={!this.state.data.executives[index].actionReady}> {exec.name}</option>
                    )}
                </select>
            </div>
        ) : "";
    }

    renderTaskSelector() {
        return this.state.ui.canSelectTask ? (
            <div>
                <select value={this.state.ui.currentTask}
                    onChange={this.onChangeExecTask}
                    disabled={!this.state.data.executives[this.state.ui.currentExec].actionReady}>
                    {this.state.data.executives[this.state.ui.currentExec].taskList.map((task, index) =>
                        <option key={index} value={index}>{task.name}</option>
                    )}
                </select>
            </div>
        ): "";
    }

    renderExecutiveInfo() {
        console.log("exec index")
        console.log(this.state.ui.execIndex)
        let exec = this.state.data.executives[this.state.ui.execIndex];
        return (
            <div>
                {exec.name}
                <br />
                {exec.text}
            </div>
        );
    }

    renderTaskInfo() {
        let task = this.state.data.taskList[this.state.ui.taskIndex];
        return (
            <div>
                Assigning an executive for: {task.name}<br />
                {task.text}
            </div>
        );
    }

    renderActionButton() {
        let notReadyText = this.state.data.executives[this.state.ui.execIndex].actionReady ? "" : "This executive has already used their action this turn.\n";
        return (
            <div>
                {notReadyText}<br />
                <button onClick={this.publicFunctions.submitModal} disabled={!this.state.data.executives[this.state.ui.execIndex].actionReady}>Assign</button>
                <button onClick={this.publicFunctions.cancelModal}>Cancel</button>
            </div>
        );
    }

    render() {
        return (
            <Modal>
                {this.renderTaskInfo()}
                {this.renderExecutiveSelector()}
                {this.renderTaskSelector()}
                {this.renderExecutiveInfo()}
                {this.renderActionButton()}
            </Modal>
        );
    }
}

export class BuyMaterials extends Component {
    constructor(props) {
        super(props);

        this.state = {
            ui: {
                currentRes: 0,
                quantity: 0,
                cost: 0,
                warnText: false
            }
        };
        //Note to self: all components should behave like this on construction. They should only receive publicObjects and whatever parent component data they need to function, and get the rest from elsewhere.
        this.execIndex = props.execIndex;

        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;
        this.settings = props.publicObjects.settings;

        this.concretePrice = this.settings.concretePrice;
        this.steelPrice = this.settings.steelPrice;
        this.glassPrice = this.settings.glassPrice;
        let execs = this.publicFunctions.getServerObject(this.constants.OBJECT_EXEC_INFO).executives;
        for (let exec of execs) {
            if (exec.currentTask === this.enums.ExecutiveTaskType.BuyMaterials) {
                switch (exec.ability) {
                    case this.enums.ExecutiveAbility.ReduceConcretePrice:
                        if (this.concretePrice !== this.settings.concretePriceDiscountPlus) {
                            this.concretePrice = this.settings.concretePriceDiscount;
                        }
                        break;
                    case this.enums.ExecutiveAbility.ReduceConcretePricePlus:
                        this.concretePrice = this.settings.concretePriceDiscountPlus;
                        break;
                    case this.enums.ExecutiveAbility.ReduceSteelPrice:
                        if (this.steelPrice !== this.settings.steelPriceDiscountPlus) {
                            this.steelPrice = this.settings.steelPriceDiscount;
                        }
                        break;
                    case this.enums.ExecutiveAbility.ReduceSteelPricePlus:
                        this.steelPrice = this.settings.steelPriceDiscountPlus;
                        break;
                    case this.enums.ExecutiveAbility.ReduceGlassPrice:
                        if (this.glassPrice !== this.settings.glassPriceDiscountPlus) {
                            this.glassPrice = this.settings.glassPriceDiscount;
                        }
                        break;
                    case this.enums.ExecutiveAbility.ReduceGlassPricePlus:
                        this.glassPrice = this.settings.glassPriceDiscountPlus;
                        break;
                }
            }
        }

        this.money = this.publicFunctions.getServerObject(this.constants.OBJECT_CURRENT_PLAYER).money;

        this.resTypes = []
        for (const prop in this.enums.MaterialType) {
            this.resTypes[this.enums.MaterialType[prop]] = prop
        }

        this.submit = this.submit.bind(this);
        this.onChangeQuant = this.onChangeQuant.bind(this);
        this.onChangeSel = this.onChangeSel.bind(this);
        this.onDecQuant = this.onDecQuant.bind(this);
        this.onIncQuant = this.onIncQuant.bind(this);

        this.publicFunctions.addModalSubmitHandler(this.submit);
    }

    materialName(material) {
        switch (material) {
            case this.resTypes.Concrete:
                return "Concrete";
            case this.resTypes.Steel:
                return "Steel";
            case this.resTypes.Glass:
                return "Glass";
        }
    }

    submit() {
        this.publicFunctions.post(`BuyMaterials`, { resType: this.state.ui.currentRes, quantity: this.state.ui.quantity });
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

    onChangeSel(e) {
        let val = e.target.value;
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                currentRes: val,
                quantity: 0,
                cost: 0,
                warnText: false
            }
        }));
    }

    updateToQuant(quant) {
        if (quant < 0) {
            quant = 0;
        }
        let warnText = false;
        let costPer = this.costPerMaterial(Number(this.state.ui.currentRes));
        let cost = quant * costPer;
        if (cost > this.money) {
            quant = Math.floor(this.money / costPer);
            warnText = true;
        }
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                quantity: quant,
                cost: cost,
                warnText: warnText
            }
        }));
    }

    costPerMaterial(materialType) {
        switch (materialType) {
            case this.enums.MaterialType.Concrete:
                return this.concretePrice;
            case this.enums.MaterialType.Steel:
                return this.steelPrice;
            case this.enums.MaterialType.Glass:
                return this.glassPrice;
        }
    }

    renderWarningText() {
        let warningText = this.state.ui.warnText ? "You entered a number that would cost more than you can afford." : "";
        return (
            <div>
                {warningText}
            </div>
        );
    }

    submitDisabled() {
        return this.state.ui.quantity == 0;
    }

    renderMaterialSelector() {
        return (
            <div>
                Your current funds: {Utility.moneyString(this.money)}<br />
                <select value={this.state.ui.currentRes} onChange={this.onChangeSel}>
                    {this.resTypes.map((material, index) =>
                        <option key={index} value={index}>{material}</option>
                    )}
                </select>
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
                {this.renderMaterialSelector()}
                {this.renderMaterialQuant()}
                <ModalSubmitCancel onSubmit={this.publicFunctions.submitModal} onCancel={this.publicFunctions.cancelModal} submitDisabled={this.submitDisabled()} />
            </Modal>
        );
    }
}

export class HireWorkers extends Component {
    constructor(props) {
        super(props);

        this.state = {
            ui: {
                quantity: 0,
                cost: 0,
                warnText: false
            },
        }

        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;
        this.settings = props.publicObjects.settings;

        this.money = this.publicFunctions.getServerObject(this.constants.OBJECT_CURRENT_PLAYER).money;

        this.onChangeWorkQuant = this.onChangeWorkQuant.bind(this);
        this.onDecWorkQuant = this.onDecWorkQuant.bind(this);
        this.onIncWorkQuant = this.onIncWorkQuant.bind(this);
        this.submit = this.submit.bind(this);

        this.publicFunctions.addModalSubmitHandler(this.submit);
    }

    onChangeWorkQuant(e) {
        let quant = Number(e.target.value);
        if (isNaN(quant)) {
            return;
        }
        this.updateToWorkQuant(quant);
    }

    onDecWorkQuant(e) {
        e.preventDefault();
        let quant = this.state.ui.quantity;
        quant--;
        this.updateToWorkQuant(quant);
    }

    onIncWorkQuant(e) {
        e.preventDefault();
        let quant = this.state.ui.quantity;
        quant++;
        this.updateToWorkQuant(quant);
    }

    updateToWorkQuant(quant) {
        if (quant < 0) {
            quant = 0;
        }
        let warnText = false;
        let costPer = 0;
        let cost = 0;
        costPer = this.settings.workerSalary;
        cost = quant * costPer;
        if (cost > this.money) {
            quant = Math.floor(this.money / costPer);
            warnText = true;
        }
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                quantity: quant,
                cost: cost,
                warnText: warnText
            }
        }));
    }

    submit() {
        this.publicFunctions.post(`HireWorkers`, { quantity: this.state.ui.quantity });
    }

    submitDisabled() {
        return this.state.ui.cost > this.money;
    }

    renderWarningText() {
        let warningText = this.state.ui.warnText ? "You entered a number that would cost more than you can afford." : "";
        return (
            <div>
                {warningText}
            </div>
        );
    }

    renderWorkerQuant() {
        return (
            <div>
                Hiring new workers will require you pay their salary now.<br />
                However, you will only be able to put them to work next turn (they're in training).<br />
                <button onClick={this.onDecWorkQuant}>-</button>
                <input type="text" pattern="[0-9]*"
                    value={this.state.ui.quantity}
                    onChange={this.onChangeWorkQuant} />
                <button onClick={this.onIncWorkQuant}>+</button>
                <br />Cost: {Utility.moneyString(this.state.ui.cost)}
            </div>
        );
    }

    render() {
        return (
            <Modal>
                {this.renderWarningText()}
                {this.renderWorkerQuant()}
                <ModalSubmitCancel onSubmit={this.publicFunctions.submitModal} onCancel={this.publicFunctions.cancelModal} submitDisabled={this.submitDisabled()} />
            </Modal>
        );
    }
}

export class FireWorkers extends Component {
    constructor(props) {
        super(props);

        this.state = {
            ui: {
                quantity: 0,
            },
        }

        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;
        this.settings = props.publicObjects.settings;

        this.curWorkers = this.publicFunctions.getServerObject(this.constants.OBJECT_CURRENT_PLAYER).totalWorkers;

        this.onChangeWorkQuant = this.onChangeWorkQuant.bind(this);
        this.onDecWorkQuant = this.onDecWorkQuant.bind(this);
        this.onIncWorkQuant = this.onIncWorkQuant.bind(this);
        this.submit = this.submit.bind(this);

        this.publicFunctions.addModalSubmitHandler(this.submit);
    }

    onChangeWorkQuant(e) {
        let quant = Number(e.target.value);
        if (isNaN(quant)) {
            return;
        }
        this.updateToWorkQuant(quant);
    }

    onDecWorkQuant(e) {
        e.preventDefault();
        let quant = this.state.ui.quantity;
        quant--;
        this.updateToWorkQuant(quant);
    }

    onIncWorkQuant(e) {
        e.preventDefault();
        let quant = this.state.ui.quantity;
        quant++;
        this.updateToWorkQuant(quant);
    }

    updateToWorkQuant(quant) {
        if (quant < 0) {
            quant = 0;
        }
        if (quant > this.curWorkers) {
            quant = this.curWorkers;
        }
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                quantity: quant
            }
        }));
    }

    submit() {
        this.publicFunctions.post(`FireWorkers`, { workersToFire: this.state.ui.quantity });
    }

    submitDisabled() {
        return this.state.ui.quantity === 0;
    }

    renderWorkerQuant() {
        return (
            <div>
                Fire workers to lower your quarterly salary costs.<br />
                Remember that newly hired workers cannot be put to work immediately.<br />
                <button onClick={this.onDecWorkQuant}>-</button>
                <input type="text" pattern="[0-9]*"
                    value={this.state.ui.quantity}
                    onChange={this.onChangeWorkQuant} />
                <button onClick={this.onIncWorkQuant}>+</button>
            </div>
        );
    }

    render() {
        return (
            <Modal>
                {this.renderWorkerQuant()}
                <ModalSubmitCancel onSubmit={this.publicFunctions.submitModal} onCancel={this.publicFunctions.cancelModal} submitDisabled={this.submitDisabled()} />
            </Modal>
        );
    }
}

export class HireExecutive extends Component {
    constructor(props) {
        super(props);

        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;
        this.settings = props.publicObjects.settings;

        this.state = {
            data: this.publicFunctions.getServerObject(this.constants.OBJECT_MODAL)
        }

        this.money = this.publicFunctions.getServerObject(this.constants.OBJECT_CURRENT_PLAYER).money;

        this.renderExecutive = this.renderExecutive.bind(this);
        this.submit = this.submit.bind(this);
        this.cancel = this.cancel.bind(this);
        this.disableSubmit = this.disableSubmit.bind(this);
    }

    renderExecutive(executive) {
        return <ExecutiveInfo executive={executive} />;
    }

    submit(executive, index) {
        this.publicFunctions.post("HireExecutive", { executiveIndex: index });
    }

    cancel() {
        this.publicFunctions.closeModal();
    }

    disableSubmit(executive) {
        return this.money < executive.salary;
    }

    render() {
        return (
            <Modal>
                <CardSelection title="Select an Executive"
                    cards={this.state.data.executives}
                    cardRenderer={this.renderExecutive}
                    selectionHandler={this.submit}
                    selectionDisabled={this.disableSubmit}
                    selectionText="Hire"
                    cancelHandler={this.cancel}
                    cancelText="Skip" />
            </Modal>
        );
    }
}

export class FindOpportunity extends Component {
    constructor(props) {
        super(props);

        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;
        this.settings = props.publicObjects.settings;

        this.state = {
            data: this.publicFunctions.getServerObject(this.constants.OBJECT_MODAL)
        }

        this.renderOpportunity = this.renderOpportunity.bind(this);
        this.submit = this.submit.bind(this);
        this.cancel = this.cancel.bind(this);
        this.disableSubmit = this.disableSubmit.bind(this);
    }

    renderOpportunity(opportunity) {
        return <OpportunityInfo opportunity={opportunity} enums={this.enums} />;
    }

    submit(opportunity, index) {
        this.publicFunctions.post("FindOpportunity", { opportunityIndex: index });
    }

    cancel() {
        this.publicFunctions.closeModal();
    }

    disableSubmit(opportunity) {
        return false;
    }

    render() {
        return (
            <Modal>
                <CardSelection title="Select an Opportunity"
                    cards={this.state.data.opportunities}
                    cardRenderer={this.renderOpportunity}
                    selectionHandler={this.submit}
                    selectionDisabled={this.disableSubmit}
                    selectionText="Add to Hand"
                    cancelHandler={this.cancel}
                    cancelText="Skip" />
            </Modal>
        );
    }
}

export class OpportunityInfo extends Component {
    constructor(props) {
        super(props);

        this.opportunity = props.opportunity;
        this.enums = props.enums;

        this.matTypes = []
        for (const prop in this.enums.MaterialType) {
            this.matTypes[this.enums.MaterialType[prop]] = prop
        }
    }

    renderOpportunityEffects() {
        switch (this.opportunity.type) {
            case this.enums.OpportunityType.MaterialDeal:
                return this.renderMaterialDealEffects();
            case this.enums.OpportunityType.PrivateProject:
                return this.renderPrivateProjectEffects();
            case this.enums.OpportunityType.ExecutiveHire:
                return this.renderExecutiveHireEffects();
            case this.enums.OpportunityType.MoneyWindfall:
                return this.renderMoneyWindfallEffects();
        }
        return "";
    }

    renderMaterialDealEffects() {
        return (
            <div>
                Buy a limited supply of {this.matTypes[this.opportunity.material]} at a special price.<br />
                Cost per unit: {Utility.moneyString(this.opportunity.price)}<br />
                Maximum order size: {this.opportunity.maxQuantity}
            </div>
        );
    }

    renderPrivateProjectEffects() {
        return (
            <ProjectStaticDisplay inProgress={false} projData={this.opportunity.project} />
        );
    }

    renderExecutiveHireEffects() {
        return (
            <div>
                Hire up to one out of {this.opportunity.executiveCount} executives.<br />
                Their salary will be {Utility.modifierToPercentage(this.opportunity.salaryAdjustment.toString())} of the usual.
            </div>
        );
    }

    renderMoneyWindfallEffects() {
        return (
            <div>
                Get {Utility.moneyString(this.opportunity.quantity)}, no strings attached. (What kind of investor has no strings attached? This kind.)
            </div>
        );
    }


    render() {
        return (
            <div>
                {this.opportunity.name}
                <br />
                {this.opportunity.description}
                <br /><br />
                {this.renderOpportunityEffects()}
            </div>
        );
    }
}