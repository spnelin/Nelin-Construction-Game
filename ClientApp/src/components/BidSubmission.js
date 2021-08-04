import React, { Component } from 'react';
import { ProjectStaticDisplay } from './Project';
import Utility from './Utility';

export class BidSubmission extends Component {
    constructor(props) {
        super(props);

        //Note to self: all components should behave like this on construction. They should only receive publicObjects and whatever parent component data they need to function, and get the rest from elsewhere.
        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;

        this.state = {
            ui: {
                currentValue: 0
            },
            data: this.publicFunctions.getServerObject(this.constants.OBJECT_BID_SESSION)
        };

        this.onBidChange = this.onBidChange.bind(this);
        this.onBidSubmit = this.onBidSubmit.bind(this);
        this.submitBid = this.submitBid.bind(this);
        this.onBidSkip = this.onBidSkip.bind(this);
        this.onBidCancel = this.onBidCancel.bind(this);
        this.updateBidSession = this.updateBidSession.bind(this);

        this.publicFunctions.addListener(this.constants.OBJECT_BID_SESSION, this.updateBidSession);
    }

    componentWillUnmount() {
        this.publicFunctions.removeListener(this.constants.OBJECT_BID_SESSION, this.updateBidSession);
    }

    updateBidSession(data) {
        this.setState({
            data: data
        });
    }

    onBidChange(e) {
        let quant = Number(e.target.value);
        if (isNaN(quant)) {
            if (quant === "") {
                this.setState(prevState => ({
                    ui: {
                        ...prevState.ui,
                        currentValue: quant
                    }
                }));
            }
            return;
        }
        if (quant > this.state.data.currentProject.maxBid) {
            quant = this.state.data.currentProject.maxBid;
        }
        if (quant < 0) {
            quant = 0;
        }
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                currentValue: quant
            }
        }));
    }

    onBidSubmit(e) {
        e.preventDefault();
        //If the bid's less than half, prompt the user to confirm
        if (this.state.ui.currentValue * 2 < this.state.data.currentProject.maxBid) {
            this.publicFunctions.launchConfirmationModal(`Your bid is ${this.state.ui.currentValue}, which is less than half of the maximum bid of ${this.state.data.currentProject.maxBid}. Are you sure you want to bid so little?`, this.submitBid);
        }
        else {
            this.submitBid();
        }
    }

    submitBid() {
        this.publicFunctions.gameActionFetch(`SubmitBid?bidQuantity=${this.state.ui.currentValue}&expectedBidIndex=${this.state.data.currentBidIndex}`);
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                currentValue: 0
            }
        }));
    }

    onBidSkip(e) {
        e.preventDefault();
        this.publicFunctions.gameActionFetch(`SubmitBid?bidQuantity=0&expectedBidIndex=${this.state.data.currentBidIndex}`);
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                currentValue: 0
            }
        }));
    }

    onBidCancel(e) {
        e.preventDefault();
        this.publicFunctions.gameActionFetch(`SubmitBid?bidQuantity=-1&expectedBidIndex=${this.state.data.currentBidIndex}`);
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                currentValue: 0
            }
        }));
    }

    renderBidStatus() {
        switch (this.state.data.currentBid) {
            case -1:
                return (
                    <div>
                        You have not yet submitted a bid.
                    </div>
                );
            case 0:
                return (
                    <div>
                        You have chosen to skip this bid.
                    </div>
                );
            default:
                return (
                    <div>
                        {`You have successfully submitted a bid of ${Utility.moneyString(this.state.data.currentBid)}.`}
                    </div>
                );
        }
    }

    renderBidEntry() {
        switch (this.state.data.currentBid) {
            case -1:
                return (
                    <div>
                        $<input type="text" value={this.state.ui.currentValue} onChange={this.onBidChange} />,000
                        <br />
                        <button onClick={this.onBidSubmit} disabled={this.state.ui.currentValue === 0}>Submit</button>
                        <button onClick={this.onBidSkip}>Skip</button>
                    </div>
                );
            default:
                return (
                    <div>
                        <button onClick={this.onBidCancel}>Cancel</button>
                    </div>
                );
        }
    }

    renderDisqualified() {
        return (
            <div>
                <ProjectStaticDisplay projData={this.state.data.currentProject} /><br />
                You cannot bid on this project: {this.state.data.disqualificationReason}<br />
                Wait until the other players are done bidding to continue.
            </div>
        );
    }

    renderBid() {
        return (
            <div>
                <b>{this.state.data.currentProject.name}</b>
                <ProjectStaticDisplay projData={this.state.data.currentProject} />
                {this.renderBidStatus()}
                {this.renderBidEntry()}
            </div>
        );
    }

    render() {
        let contents = this.state.data.disqualifiedFromBidding ? this.renderDisqualified() : this.renderBid();

        return contents;
    }
}