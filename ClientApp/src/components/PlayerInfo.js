import React, { Component } from 'react';
import Utility from './Utility';
import { ProjectList } from './Project';
import { FireWorkers } from './ExecutiveAssignment';

export class PlayerInfoDisplay extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                Money: {Utility.moneyString(this.props.player.money)}<br />
                Concrete: {this.props.player.concrete}<br />
                Steel: {this.props.player.steel}<br />
                Glass: {this.props.player.glass}<br />
                Free Workers: {this.props.player.freeWorkers}/{this.props.player.totalWorkers}
            </div>
        );
    }
}

export class PlayerInfoBar extends Component {
    constructor(props) {
        super(props);

        console.log(props)

        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;

        this.state = {
            data: this.publicFunctions.getServerObject(this.constants.OBJECT_CURRENT_PLAYER)
        };

        this.onClickReady = this.onClickReady.bind(this);
        this.updatePlayer = this.updatePlayer.bind(this);

        this.launchFireWorkersModal = this.launchFireWorkersModal.bind(this);
        this.renderFireWorkersModal = this.renderFireWorkersModal.bind(this);

        this.publicFunctions.addListener(this.constants.OBJECT_CURRENT_PLAYER, this.updatePlayer);
    }

    componentWillUnmount() {
        this.publicFunctions.removeListener(this.constants.OBJECT_CURRENT_PLAYER, this.updatePlayer);
    }

    updatePlayer(data) {
        this.setState({ data: data });
    }

    onClickReady(e) {
        e.preventDefault();
        this.publicFunctions.setReadyToAdvance(!this.state.data.isReadyToAdvance);
    }

    launchFireWorkersModal(e) {
        e.preventDefault();
        this.publicFunctions.launchCustomModal(this.renderFireWorkersModal);
    }

    renderFireWorkersModal() {
        return <FireWorkers publicObjects={this.publicFunctions.getPublicObjects()} />;
    }

    render() {
        const container = {
            //height: "15%",
            width: "100%",
            height: "100%",
            display: "flex",
            flexDirection: "row",
            position: "relative"
        }
        const component = {
            height: "100%",
            minHeight: "100%",
            padding: "10px",
            //margin: "10px",
            flexGrow: "1",
            border: "2px solid black",
            display: "flex",
            flexDirection: "row"
        }
        const subComponent = {
            padding: "10px"
        }
        return (
            <div style={container}>
                <div style={component}>
                    {this.state.data.name}, Inc.<br />
                    Game ID: {this.publicFunctions.getGameID()}
                </div>
                <div style={component}>
                    Money: {Utility.moneyString(this.state.data.money)}
                </div>
                <div style={component}>
                    <div style={subComponent}>
                        Concrete: {this.state.data.concrete}
                    </div>
                    <div style={subComponent}>
                        Steel: {this.state.data.steel}
                    </div>
                    <div style={subComponent}>
                        Glass: {this.state.data.glass}
                    </div>
                    <div style={subComponent}>
                        <button onClick={() => this.publicFunctions.executiveTask(this.enums.ExecutiveTaskType.BuyMaterials)}>{this.state.data.buyMaterialsUnlocked ? "" : "\u26BF"}Buy</button>
                    </div>
                </div>
                <div style={component}>
                    <div style={subComponent}>
                        Workers: {this.state.data.freeWorkers}/{this.state.data.totalWorkers}
                    </div>
                    <div style={subComponent}>
                        <button onClick={() => this.publicFunctions.executiveTask(this.enums.ExecutiveTaskType.HireWorkers)}>{this.state.data.hireWorkersUnlocked ? "" : "\u26BF"}Hire</button>
                    </div>
                    <div style={subComponent}>
                        <button onClick={this.launchFireWorkersModal} disabled={this.state.data.totalWorkers === 0}>Fire</button>
                    </div>
                </div>
                <div style={component}>
                    Advance: <button onClick={this.onClickReady}>{this.state.data.isReadyToAdvance ? "Unready" : "Ready"}</button>
                </div>
            </div>
        );
    }
}

export class PlayerInfo extends Component {
    constructor(props) {
        super(props);

        console.log(props)

        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;

        this.state = {
            data: this.publicFunctions.getServerObject(this.constants.OBJECT_CURRENT_PLAYER)
        };

        this.onClickReady = this.onClickReady.bind(this);
        this.onClickViewCompProjects = this.onClickViewCompProjects.bind(this);
        this.renderMyCompProjListModal = this.renderMyCompProjListModal.bind(this);
        this.updatePlayer = this.updatePlayer.bind(this);

        this.launchFireWorkersModal = this.launchFireWorkersModal.bind(this);
        this.renderFireWorkersModal = this.renderFireWorkersModal.bind(this);

        this.publicFunctions.addListener(this.constants.OBJECT_CURRENT_PLAYER, this.updatePlayer);
    }

    componentWillUnmount() {
        this.publicFunctions.removeListener(this.constants.OBJECT_CURRENT_PLAYER, this.updatePlayer);
    }

    updatePlayer(data) {
        this.setState({ data: data });
    }

    onClickReady(e) {
        e.preventDefault();
        this.publicFunctions.setReadyToAdvance(!this.state.data.isReadyToAdvance);
    }

    onClickViewCompProjects(e) {
        e.preventDefault();
        this.publicFunctions.launchOkModal(this.renderMyCompProjListModal);
    }

    renderMyCompProjListModal() {
        return (
            <div>
                <ProjectList projects={this.state.data.completedProjects} inProgress={false} />
            </div>
        );
    }

    launchFireWorkersModal(e) {
        e.preventDefault();
        this.publicFunctions.launchCustomModal(this.renderFireWorkersModal);
    }

    renderFireWorkersModal() {
        return <FireWorkers publicObjects={this.publicFunctions.getPublicObjects()} />;
    }

    renderReadyButton() {
        return (
            <div key="2">
                Ready: <button onClick={this.onClickReady}>{this.state.data.isReadyToAdvance ? "Unready" : "Ready"}</button>
            </div>
        );
    }

    renderReadyStatus() {
        return (
            <div key="3">
                Ready to advance: {this.state.data.isReadyToAdvance ? "Yes" : "No"}
            </div>
        );
    }

    renderCurrProjectsButton() {
        return this.state.data.completedProjects.length > 0 ? (
            <div key="4">
                Completed Projects: <button onClick={this.onClickViewCompProjects}>View</button>
            </div>
        ) : "";
    }

    render() {
        let rows = [];
        rows.push(
            <div key="1">
                Completed Projects: <button onClick={this.onClickViewCompProjects}>View</button>
            </div>
        );
        return (
            <div>
                <PlayerInfoDisplay player={this.state.data} />
                {rows}
            </div>
        );
    }
}

export class OtherPlayerList extends Component {
    constructor(props) {
        super(props);

        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;

        this.state = {
            ui: {
                currentPlayer: 0
            },
            data: this.publicFunctions.getServerObject(this.constants.OBJECT_OTHER_PLAYERS)
        };

        this.onChangeOthPlayerSelection = this.onChangeOthPlayerSelection.bind(this);
        this.onClickViewOthCompProj = this.onClickViewOthCompProj.bind(this);
        this.renderOthCompProjListModal = this.renderOthCompProjListModal.bind(this);
        this.renderOthCurrProjListModal = this.renderOthCurrProjListModal.bind(this);
        this.updateOtherPlayers = this.updateOtherPlayers.bind(this);

        this.publicFunctions.addListener(this.constants.OBJECT_OTHER_PLAYERS, this.updateOtherPlayers);
    }

    componentWillUnmount() {
        this.publicFunctions.removeListener(this.constants.OBJECT_OTHER_PLAYERS, this.updateOtherPlayers);
    }

    updateOtherPlayers(data) {
        this.setState({ data: data });
    }

    onChangeOthPlayerSelection(e) {
        let newIndex = e.target.value;
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                currentPlayer: newIndex
            }
        }))
    }

    onClickViewOthCurrProj(e) {
        e.preventDefault();
        this.publicFunctions.launchOkModal(this.renderOthCurrProjListModal);
    }

    onClickViewOthCompProj(e) {
        e.preventDefault();
        this.publicFunctions.launchOkModal(this.renderOthCompProjListModal);
    }

    renderOthCurrProjListModal() {
        return (
            <div>
                <ProjectList projects={this.state.data[this.state.ui.currentPlayer].currentProjects} inProgress="true" />
            </div>
        );
    }

    renderOthCompProjListModal() {
        return (
            <div>
                <ProjectList projects={this.state.data[this.state.ui.currentPlayer].completedProjects} inProgress="false" />
            </div>
        );
    }

    render() {
        return this.state.data && this.state.data.length > 0 ? (
            <div>
                <select value={this.state.ui.currentPlayer}
                    onChange={this.onChangePlayerSelection}>
                    {this.state.data.map((player, index) =>
                        <option key={index} value={index}>{player.name}</option>
                    )}
                </select>
                <PlayerInfoDisplay player={this.state.data[this.state.ui.currentPlayer]} /><br />
                Ready to bid: {this.state.data[this.state.ui.currentPlayer].isReadyToAdvance ? "Yes" : "No"}<br />
                {this.state.data[this.state.ui.currentPlayer].currentProjects.length === 0 ? "" : <button onClick={this.onClickViewOthCurrProj}>Current Projects</button>}<br />
                {this.state.data[this.state.ui.currentPlayer].completedProjects.length === 0 ? "" : <button onClick={this.onClickViewOthCompProj}>Completed Projects</button>}
            </div>
        ) : "";
    }
}