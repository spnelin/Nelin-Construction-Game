import React, { Component } from 'react';
import { Chat, ChatCreator } from './Chat'
import { CardSelection } from './CardSelection';
import { PlayerInfo, OtherPlayerList, PlayerInfoBar } from './PlayerInfo';
import { Log } from './Log';
import { BidSubmission } from './BidSubmission';
import { ExecutiveAssignment, ExecutiveBuyMaterials, ExecutiveHireWorkers, ExecutiveHireExecutive, HireExecutive, BuyMaterials, HireWorkers, FindOpportunity } from './ExecutiveAssignment';
import { Modal, ModalOk, ModalSubmitCancel } from './Modal';
import { ProjectStaticDisplay, ProjectWorkInterface, ProjectCurrent, ProjectUpcoming } from './Project';
import Utility from './Utility';
import { CostReport } from './CostReport';
import Constants from './Constants';
import { CardHandMinimized, MaterialDealOpportunity, CardHand } from './Opportunity';
import { ExecutiveList } from './Executive';

export class SiteCore extends Component {

    constructor(props) {
        super(props);
        //bind functions to this
        this.onChangeUserNameEntry = this.onChangeUserNameEntry.bind(this);
        this.onUserNameKeyDown = this.onUserNameKeyDown.bind(this);
        this.onClickUserNameSubmit = this.onClickUserNameSubmit.bind(this);
        this.onChangeGameIdEntry = this.onChangeGameIdEntry.bind(this);
        this.onGameIdEntryKeyDown = this.onGameIdEntryKeyDown.bind(this);
        this.onClickGameIdSubmit = this.onClickGameIdSubmit.bind(this);
        this.onClickNewGame = this.onClickNewGame.bind(this);
        this.onClickBecomePlayer = this.onClickBecomePlayer.bind(this);
        this.postAuthenticate = this.postAuthenticate.bind(this);
        this.joinGame = this.joinGame.bind(this);
        this.loadGame = this.loadGame.bind(this);
        this.loadEnums = this.loadEnums.bind(this);
        this.loadConstants = this.loadConstants.bind(this);
        this.loadRevision = this.loadRevision.bind(this);
        this.loadUpdateRevision = this.loadUpdateRevision.bind(this);
        this.nullHandler = this.nullHandler.bind(this);
        this.processObjects = this.processObjects.bind(this);
        this.submitModal = this.submitModal.bind(this);
        this.cancelModal = this.cancelModal.bind(this);
        this.onClickReady = this.onClickReady.bind(this);

        this.launchCostReportModal = this.launchCostReportModal.bind(this);
        this.costReportViewed = this.costReportViewed.bind(this);
        this.renderCostReportModal = this.renderCostReportModal.bind(this);
        this.launchProjectCompletionModal = this.launchProjectCompletionModal.bind(this);
        this.renderProjectCompletionModal = this.renderProjectCompletionModal.bind(this);
        this.launchHireExecutiveModal = this.launchHireExecutiveModal.bind(this);
        this.renderHireExecutiveModal = this.renderHireExecutiveModal.bind(this);
        this.renderBuyMaterialsModal = this.renderBuyMaterialsModal.bind(this);
        this.renderHireWorkersModal = this.renderHireWorkersModal.bind(this);
        this.launchFindOpportunityModal = this.launchFindOpportunityModal.bind(this);
        this.renderFindOpportunityModal = this.renderFindOpportunityModal.bind(this);
        this.launchMaterialDealModal = this.launchMaterialDealModal.bind(this);
        this.renderMaterialDealModal = this.renderMaterialDealModal.bind(this);

        //load constants and enums
        this.genericFetch(`api/Action/GetConstants`, this.loadConstants);
        this.genericFetch(`api/Action/GetEnums`, this.loadEnums);

        //initialize state - start at Authentication
        this.state = {
            isAuthenticated: false,
            enteredUserName: "",
            currentTime: new Date(0),
            updateInterval: null,
            modalActive: false,
            otherPlayers: null,
            owningPlayer: null,
            gameLog: null
        };

        this.serverObjects = [];

        this.getServerObject = this.getServerObject.bind(this);
        this.getPublicObjects = this.getPublicObjects.bind(this);
        this.getGameID = this.getGameID.bind(this);

        this.eventHolder = { test: [] };
        this.addListener = this.addListener.bind(this);
        this.removeListener = this.removeListener.bind(this);
        this.listenerAdder = this.listenerAdder.bind(this);
        this.listenerRemover = this.listenerRemover.bind(this);
        this.invokeEvent = this.invokeEvent.bind(this);

        this.launchCustomModal = this.launchCustomModal.bind(this);
        this.launchOkModal = this.launchOkModal.bind(this);
        this.launchSubmitModal = this.launchSubmitModal.bind(this);
        this.launchConfirmationModal = this.launchConfirmationModal.bind(this);
        this.addModalCancelHandler = this.addModalCancelHandler.bind(this);
        this.addModalSubmitHandler = this.addModalSubmitHandler.bind(this);
        this.closeModal = this.closeModal.bind(this);
        this.clearModal = this.clearModal.bind(this);

        this.executiveTask = this.executiveTask.bind(this);

        this.gameActionFetch = this.gameActionFetch.bind(this);
        this.subComponentGameActionFetch = this.subComponentGameActionFetch.bind(this);
        this.subComponentPost = this.subComponentPost.bind(this);

        this.setReadyToAdvance = this.setReadyToAdvance.bind(this);

        this.publicObjects = {
            functions: {
                getServerObject: this.getServerObject,
                getPublicObjects: this.getPublicObjects,
                getGameID: this.getGameID,
                gameActionFetch: this.subComponentGameActionFetch, //subcomponents should have part of the fetching abstracted away
                post: this.subComponentPost,
                addListener: this.addListener,
                removeListener: this.removeListener,
                launchCustomModal: this.launchCustomModal,
                launchOkModal: this.launchOkModal,
                launchSubmitModal: this.launchSubmitModal,
                launchConfirmationModal: this.launchConfirmationModal,
                addModalCancelHandler: this.addModalCancelHandler,
                addModalSubmitHandler: this.addModalSubmitHandler,
                submitModal: this.submitModal,
                cancelModal: this.cancelModal,
                setReadyToAdvance: this.setReadyToAdvance,
                closeModal: this.closeModal,
                executiveTask: this.executiveTask
            },
            renders: {
                //renderChatCreateModal: this.renderChatCreateModal
            },
            settings: {}
        }
        console.log(this.publicObjects);
    }

    //Getting server objects
    getServerObject(identifier) {
        return this.serverObjects[identifier];
    }

    getPublicObjects() {
        return this.publicObjects;
    }

    getGameID() {
        return this.state.gameId;
    }

    //Event handling
    addListener(eventType, listener) {
        this.eventHolder[eventType].push(listener);
    }

    removeListener(eventType, listener) {
        this.eventHolder[eventType].splice(this.eventHolder[eventType].indexOf(listener), 1);
    }

    listenerAdder(eventType) {
        return listener => this.addListener(eventType, listener);
    }

    listenerRemover(eventType) {
        return listener => this.removeListener(eventType, listener);
    }

    invokeEvent(eventType, eventArgs) {
        console.log("Invoking events for " + eventType)
        console.log(this.eventHolder[eventType])
        for (let i = 0; i < this.eventHolder[eventType].length; i++) {
            this.eventHolder[eventType][i](eventArgs);
        }
    }

    //Constant value loading
    loadConstants(data) {
        this.setState({ constants: data });
        this.eventHolder[data.OBJECT_GAME] = [];
        this.eventHolder[data.OBJECT_CHAT] = [];
        this.eventHolder[data.OBJECT_CURRENT_PLAYER] = [];
        this.eventHolder[data.OBJECT_OTHER_PLAYERS] = [];
        this.eventHolder[data.OBJECT_CURRENT_PROJECTS] = [];
        this.eventHolder[data.OBJECT_EXEC_INFO] = [];
        this.eventHolder[data.OBJECT_GAME_LOG] = [];
        this.eventHolder[data.OBJECT_PROJECT_PIPELINE] = [];
        this.eventHolder[data.OBJECT_BID_SESSION] = [];
        this.eventHolder[data.OBJECT_MODAL] = [];
        this.eventHolder[data.OBJECT_HAND] = [];
        this.publicObjects["constants"] = data;
    }

    loadEnums(data) {
        this.setState({ enums: data });
        this.publicObjects["enums"] = data;
    }

    //Generic Fetch
    genericFetch(path, successHandler) {
        console.log(`Fetching from ${path}`)
        return fetch(path)
            .then(response => this.handleResponse(response))
            .then(data => successHandler(data))
            .catch(reason => this.handleRejectedRequest(reason, path));
    }

    post(path, data, successHandler) {
        console.log(`Posting to ${path} with the following data:`)
        console.log(data)
        return fetch(path, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
            })
            .then(response => this.handleResponse(response))
            .then(data => successHandler(data))
            .catch(reason => this.handleRejectedRequest(reason, path));
    }

    subComponentPost(path, data) {
        data["gameId"] = this.state.gameId;
        this.post(`api/Action/${path}`, data, this.nullHandler);
    }

    updateFetch() {
        this.genericFetch(`api/Action/${this.state.gameId}/Update`, this.loadUpdateRevision);
    }

    gameActionFetch(path) {
        this.genericFetch(path, this.nullHandler);
    }

    subComponentGameActionFetch(path) {
        this.genericFetch(`api/Action/${this.state.gameId}/${path}`, this.nullHandler);
    }

    nullHandler() {
        //nothing
    }

    handleResponse(response) {
        console.log(`Response:`)
        console.log(response)
        if (response.ok) {
            return response.json();
        }
        else {
            throw new Error(`Error ${response.status}: ${response.statusTest} - ${response.text()}`);
        }
    }

    handleRejectedRequest(reason, path) {
        console.log(`Could not resolve request to ${path} for reason: ${reason}`);
        console.log("Stack trace:");
        console.log(reason.stack);
        //console.log(`Kicking this session back out to authentication to clean up state.`);
        //this.setState({ isAuthenticated: false });
    }

    //Authentication Handlers
    onChangeUserNameEntry(e) {
        let val = e.target.value;
        if (val.length > 1000) {
            val = val.substring(0, 999);
        }
        this.setState({ enteredUserName: val });
    }
    
    onUserNameKeyDown(e) {
        if (e.key === 'Enter') {
            let userName = this.state.enteredUserName;
            this.authenticate(userName);
        }
    }

    onClickUserNameSubmit(e) {
        e.preventDefault();
        let userName = this.state.enteredUserName;
        this.authenticate(userName);
    }

    authenticate(userName) {
        this.genericFetch(`api/Action/Authenticate?user=${encodeURIComponent(userName)}&password=please`, this.postAuthenticate);
    }

    postAuthenticate(data) {
        //set props to prepare for Game Selection screen
        //todo: load things like "games this user is already in"
        this.setState({ isAuthenticated: true, enteredUserName: "", userName: data.userName, gameId: 0, enteredGameId: "" });
    }

    //Game Selection Handlers
    onChangeGameIdEntry(e) {
        let val = e.target.value;
        if (val.length > 1000) {
            val = val.substring(0, 999);
        }
        this.setState({ enteredGameId: val });
    }

    onGameIdEntryKeyDown(e) {
        if (e.key === 'Enter') {
            let gameId = this.state.enteredGameId;
            this.joinGame(gameId);
        }
    }

    onClickGameIdSubmit(e) {
        e.preventDefault();
        let gameId = this.state.enteredGameId;
        this.requestJoinGame(gameId);
    }

    onClickNewGame(e) {
        e.preventDefault();
        this.genericFetch(`api/Action/CreateGame`, (data) => this.joinGame(data.id));
    }

    onClickReady(e) {
        e.preventDefault();
        this.setReadyToAdvance(!this.state.owningPlayer.isReadyToAdvance);
    }

    requestJoinGame(gameId) {
        this.genericFetch(`api/Action/${gameId}/RequestJoinGame`, (data) => { this.handleRequestJoinGame(data, gameId) });
    }

    handleRequestJoinGame(gameJoinStatus, gameId) {
        switch (gameJoinStatus) {
            case this.state.enums.GameJoinStatus.Join:
                this.joinGame(gameId);
                break;
            default:
                alert("Could not join game.");
                break;
        }
    }

    joinGame(gameId) {
        this.genericFetch(`api/Action/${gameId}/LoadGame`, this.loadGame);
        //todo: this next action should actually be setting up all object defaults, so we don't crash later
        this.setState({ gameId: gameId });
    }

    onClickBecomePlayer(e) {
        e.preventDefault();
        console.log("Became player");
        this.genericFetch(`api/Action/${this.state.gameId}/TryBecomePlayer`, this.loadGame);
    }

    setReadyToAdvance(ready) {
        if (ready) {
            let expectedNextState = this.state.expectedNextState;
            this.post(`api/Action/ReadyAdvance`, { gameId: this.state.gameId, expectedNextState: expectedNextState }, this.nullHandler);
        }
        else {
            this.gameActionFetch(`api/Action/${this.state.gameId}/UnreadyAdvance`)
        }
    }

    //content loading
    loadGame(data) {
        console.log("Settings:");
        console.log(data.settings);
        this.publicObjects.settings = data.settings;
        this.loadUpdateRevision(data);
    }

    loadUpdateRevision(data) {
        this.updating = false;
        this.loadRevision(data);
        if (data.stopUpdating) {
            return;
        }
        this.updateFetch();
    }

    loadRevision(data) {
        console.log("Data:");
        console.log(data);
        this.processObjects(data.changedObjects);
        this.setState({ gameState: data.gameState, playerState: data.playerState, expectedNextState: data.expectedNextState, currentTime: data.utcTime });
    }

    processObjects(changedObjects) {
        console.log("ChangedObjects:")
        console.log(changedObjects);
        changedObjects.forEach(obj => {
            if (obj === null) { return; }
            let linkedObject = obj.object;
            console.log(obj);
            switch (obj.identifier) {
                case this.state.constants.OBJECT_PLAYER_INFO:
                    //split out the data pieces returned in PlayerInfo (todo: actually split this data out on the server side)
                    console.log("Invoking other players")
                    this.invokeEvent(this.state.constants.OBJECT_OTHER_PLAYERS, linkedObject.otherPlayers);
                    this.serverObjects[this.state.constants.OBJECT_OTHER_PLAYERS] = linkedObject.otherPlayers;
                    this.setState({ otherPlayers: linkedObject.otherPlayers });
                    console.log("Invoking current player")
                    this.invokeEvent(this.state.constants.OBJECT_CURRENT_PLAYER, linkedObject.owningPlayer);
                    this.serverObjects[this.state.constants.OBJECT_CURRENT_PLAYER] = linkedObject.owningPlayer;
                    this.setState({ owningPlayer: linkedObject.owningPlayer });
                    console.log("Invoking projects")
                    this.invokeEvent(this.state.constants.OBJECT_CURRENT_PROJECTS, linkedObject.owningPlayer.currentProjects);
                    this.serverObjects[this.state.constants.OBJECT_CURRENT_PROJECTS] = linkedObject.owningPlayer.currentProjects;
                    break;
                case this.state.constants.OBJECT_EXEC_INFO:
                    this.invokeEvent(this.state.constants.OBJECT_EXEC_INFO, linkedObject);
                    this.serverObjects[this.state.constants.OBJECT_EXEC_INFO] = linkedObject;
                    break;
                case this.state.constants.OBJECT_GAME_LOG:
                    this.invokeEvent(this.state.constants.OBJECT_GAME_LOG, linkedObject);
                    this.serverObjects[this.state.constants.OBJECT_GAME_LOG] = linkedObject;
                    this.setState({ gameLog: linkedObject });
                    break;
                case this.state.constants.OBJECT_PROJECT_PIPELINE:
                    this.invokeEvent(this.state.constants.OBJECT_PROJECT_PIPELINE, linkedObject);
                    this.serverObjects[this.state.constants.OBJECT_PROJECT_PIPELINE] = linkedObject;
                    break;
                case this.state.constants.OBJECT_GAME:
                    this.invokeEvent(this.state.constants.OBJECT_GAME, linkedObject);
                    this.serverObjects[this.state.constants.OBJECT_GAME] = linkedObject;
                    break;
                case this.state.constants.OBJECT_BID_SESSION:
                    this.invokeEvent(this.state.constants.OBJECT_BID_SESSION, linkedObject);
                    this.serverObjects[this.state.constants.OBJECT_BID_SESSION] = linkedObject;
                    break;
                case this.state.constants.OBJECT_CHAT:
                    this.invokeEvent(this.state.constants.OBJECT_CHAT, linkedObject);
                    this.serverObjects[this.state.constants.OBJECT_CHAT] = linkedObject;
                    break;
                case this.state.constants.OBJECT_HAND:
                    this.invokeEvent(this.state.constants.OBJECT_HAND, linkedObject);
                    this.serverObjects[this.state.constants.OBJECT_HAND] = linkedObject;
                    break;
                case this.state.constants.OBJECT_MODAL:
                    let ui = {};
                    let delegates = {};
                    let modalLaunch = this.nullHandler;
                    this.invokeEvent(this.state.constants.OBJECT_MODAL, linkedObject);
                    this.serverObjects[this.state.constants.OBJECT_MODAL] = linkedObject;
                    switch (linkedObject.type) {
                        case this.state.enums.ModalType.None:
                            this.closeModal();
                            break;
                        case this.state.enums.ModalType.CostReport:
                            ui = this.getCostReportUi();
                            delegates = this.getCostReportDelegates();
                            modalLaunch = this.launchCostReportModal;
                            break;
                        case this.state.enums.ModalType.ProjectCompletion:
                            ui = {};
                            delegates = {};
                            modalLaunch = this.launchProjectCompletionModal;
                            break;
                        case this.state.enums.ModalType.ExecHire:
                            ui = {};
                            delegates = {};
                            modalLaunch = this.launchHireExecutiveModal;
                            break;
                        case this.state.enums.ModalType.OpportunitySearch:
                            ui = {};
                            delegates = {};
                            modalLaunch = this.launchFindOpportunityModal;
                            break;
                        case this.state.enums.ModalType.MaterialDeal:
                            ui = {};
                            delegates = {};
                            modalLaunch = this.launchMaterialDealModal;
                            break;
                        default:
                            //might need to do some no-modal-needed cleanup later
                            break;
                    }
                    this.setState({
                        modalObj: {
                            ui: ui,
                            data: linkedObject,
                            delegates: delegates
                        }
                    });
                    modalLaunch();
                    break;
            }
        });
    }

    //initialize empty objects (for while we're loading the game initially?)
    initializeModalObj() {
        return {
            ui: {},
            data: {},
            delegates: {}
        }
    }

    //Cost report
    costReportViewed() {
        this.gameActionFetch(`api/Action/${this.state.gameId}/ViewCostReport`);
    }

    getCostReportUi() {
        return {};
    }

    getCostReportDelegates() {
        return {};
    }

    //Task behavior
    getTaskIndex(type) {
        for (let index = 0; index < this.serverObjects[this.state.constants.OBJECT_EXEC_INFO].taskList.length; index++) {
            if (this.serverObjects[this.state.constants.OBJECT_EXEC_INFO].taskList[index].type === type) {
                return index;
            }
        }
        return -1;
    }

    executiveTask(task) {
        switch (task) {
            case this.state.enums.ExecutiveTaskType.BuyMaterials:
            case this.state.enums.ExecutiveTaskType.HireWorkers:
                for (let executive of this.serverObjects[this.state.constants.OBJECT_EXEC_INFO].executives) {
                    if (executive.currentTask === task) {
                        switch (task) {
                            case this.state.enums.ExecutiveTaskType.BuyMaterials:
                                this.launchBuyMaterialsModal();
                                return;
                            case this.state.enums.ExecutiveTaskType.HireWorkers:
                                this.launchHireWorkersModal();
                                return;
                        }
                        break;
                    }
                }
                //no break - if not found, continue to next option
            case this.state.enums.ExecutiveTaskType.HireExecutive:
            case this.state.enums.ExecutiveTaskType.FindOpportunity:
                this.launchExecutiveAssignmentModal(null, this.getTaskIndex(task));
                break;
        }
    }

    //Modal handling
    closeModal() {
        console.log("Closing modal")
        if (this.state.modalObj && this.state.modalObj.data && this.state.modalObj.data.type !== this.state.enums.ModalType.None) {
            this.clearModal(this.state.modalObj.data.type);
        }
        this.setState({
            modalActive: false,
            modalObj: {}
        });
    }

    clearModal(type) {
        this.gameActionFetch(`api/Action/${this.state.gameId}/ClearModal?phase=${this.state.gameState}&type=${type}`)
    }

    launchCustomModal(contentsHandler) {
        this.setState({
            modalActive: true,
            modalContents: contentsHandler
        });
    }

    launchSubmitModal(contentsHandler, submitHandler, cancelHandler, submitDisabledHandler, cancelDisabledHandler) {
        this.setState({
            modalActive: true,
            modalContents: this.getSubmitModalRenderer(contentsHandler, submitDisabledHandler, cancelDisabledHandler),
            modalSubmitDisabledHandler: submitDisabledHandler,
            modalCancelDisabledHandler: cancelDisabledHandler
        });
        this.modalSubmitHandler = submitHandler;
        this.modalCancelHandler = cancelHandler;
    }

    launchOkModal(contentsRenderer, okHandler) {
        this.setState({
            modalActive: true,
            modalContents: this.getOkModalRenderer(contentsRenderer)
        });
        this.modalSubmitHandler = okHandler;
    }

    launchConfirmationModal(query, submitHandler, cancelHandler) {
        this.launchSubmitModal(() => this.renderConfirmationModal(query), submitHandler, cancelHandler);
    }

    launchCostReportModal() {
        this.launchOkModal(this.renderCostReportModal, this.costReportViewed);
    }

    launchProjectCompletionModal() {
        this.launchOkModal(() => this.renderProjectCompletionModal(this.state.modalObj.data.project), () => this.clearModal(this.state.enums.ModalType.ProjectCompletion));
    }

    launchHireExecutiveModal() {
        this.launchCustomModal(this.renderHireExecutiveModal);
    }

    launchBuyMaterialsModal() {
        this.launchCustomModal(this.renderBuyMaterialsModal);
    }

    launchHireWorkersModal() {
        this.launchCustomModal(this.renderHireWorkersModal);
    }

    launchFindOpportunityModal() {
        this.launchCustomModal(this.renderFindOpportunityModal);
    }

    launchMaterialDealModal() {
        this.launchCustomModal(this.renderMaterialDealModal);
    }

    launchExecutiveAssignmentModal(execIndex, taskIndex) {
        this.launchCustomModal(() => this.renderExecutiveAssignmentModal(execIndex, taskIndex));
    }

    submitModal() {
        console.log("Submitted modal");
        this.setState({
            modalActive: false
        });
        if (this.modalSubmitHandler) {
            console.log("Running handler");
            console.log(this.modalSubmitHandler);
            this.modalSubmitHandler();
        }
    }

    cancelModal() {
        console.log("Canceled modal")
        this.setState({
            modalActive: false
        });
        if (this.modalCancelHandler) { this.modalCancelHandler(); }
    }

    addModalSubmitHandler(handler) {
        console.log("Adding submit handler");
        console.log(handler);
        this.modalSubmitHandler = handler;
    }

    addModalCancelHandler(handler) {
        console.log("Adding cancel handler");
        console.log(handler);
        this.modalCancelHandler = handler;
    }

    //Renders
    renderModal() {
        return this.state.modalActive ? this.state.modalContents(this) : (<div />);
    }

    getOkModalRenderer(contentsHandler) {
        return () => (
            <Modal>
                {contentsHandler()}
                <br />
                <ModalOk onSubmit={this.submitModal} />
            </Modal>
            );
    }

    getSubmitModalRenderer(contentsHandler) {
        return () => (
            <Modal>
                {contentsHandler()}
                <br />
                <ModalSubmitCancel onSubmit={this.submitModal} onCancel={this.cancelModal} />
            </Modal>
        );
    }

    renderConfirmationModal(query) {
        return (
            <div>
                {query}
            </div>
        );
    }

    renderCostReportModal() {
        return (
            <div>
                <CostReport modalObj={this.state.modalObj} />
            </div>
        );
    }

    renderProjectCompletionModal(project) {
        return (
            <div>
                Congratulations! You have completed a project:<br />
                <ProjectStaticDisplay projData={project} inProgress={false} /><br />
                This project will add to your reputation, allowing you to take on more difficult projects in the future.
            </div>
        );
    }

    renderHireExecutiveModal() {
        return (<HireExecutive publicObjects={this.publicObjects} />);
    }

    renderBuyMaterialsModal() {
        return (<BuyMaterials publicObjects={this.publicObjects} />);
    }

    renderHireWorkersModal() {
        return (<HireWorkers publicObjects={this.publicObjects} />);
    }

    renderFindOpportunityModal() {
        return <FindOpportunity publicObjects={this.publicObjects} />;
    }

    renderMaterialDealModal() {
        return <MaterialDealOpportunity publicObjects={this.publicObjects} />;
    }

    renderExecutiveAssignmentModal(execIndex, taskIndex) {
        return (<ExecutiveAssignment execIndex={execIndex} taskIndex={taskIndex} publicObjects={this.publicObjects} />);
    }

    renderAuthPage() {
        return (
            <div>
                Enter a username (try not to use someone else's username - they'll get booted out):
                <input type="text" value={this.state.enteredUserName} onChange={this.onChangeUserNameEntry} onKeyDown={this.onUserNameKeyDown} />
                <button onClick={this.onClickUserNameSubmit} disabled={this.state.enteredUserName === ""}> Submit</button>
            </div>
        );
    }

    renderGameSelectPage() {
        return (
            <div>
                Enter a game ID or create a new game:
                <input type="text" value={this.state.enteredGameId} onChange={this.onChangeGameIdEntry} onKeyDown={this.onGameIdEntryKeyDown} />
                <button onClick={this.onClickGameIdSubmit}>Join</button>
                <button onClick={this.onClickNewGame}>Create Game</button>
            </div>
        );
    }

    renderPreGamePlayerTable() {
        return (
            <table>
                <tbody>
                    {this.state.otherPlayers.map((player, index) => 
                        <tr key={index}>
                            <td>{player.name}: </td>
                            <td>{player.isReadyToAdvance ? "Ready" : "Unready"}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    renderPreGamePhase() {
        let playerTable = (this.state.otherPlayers && this.state.otherPlayers.length > 0) ? this.renderPreGamePlayerTable() : "";
        let btn = this.state.playerState === this.state.enums.PlayerState.Player && this.state.owningPlayer !== null ? <button onClick={this.onClickReady}>{this.state.owningPlayer.isReadyToAdvance ? "Unready" : "Ready"}</button> : <button onClick={this.onClickBecomePlayer}>Become Player</button>;
        return (
            <div>
                Game ID: {this.state.gameId}<br />
                {playerTable}<br />
                {btn}
            </div>
        );
    }

    renderBuildPhase() {
        const container = {
            width: "100vw",
            height: "100vh",
            display: "flex",
            flexDirection: "column",
            zIndex: 1
        }
        const activeSpace = {
            width: "100%",
            height: "77%",
            display: "flex",
            flexDirection: "row"
        }
        const logSpace = {
            width: "15%",
            height: "100%",
            minWidth: "150px",
            display: "flex",
            flexDirection: "column"
        }
        const infoBarSpace = {
            width: "100%",
            height: "8%",
            minHeight: "80px",
            border: "5px solid black"
        }
        const mainSpace = {
            width: "85%",
            height: "100%"
        }
        const handSpace = {
            width: "100%",
            height: "15%",
            minHeight: "100px",
            border: "5px solid black"
        }
        const grid = {
            display: "grid",
            gridTemplateColumns: "1fr 1fr",
            gridTemplateRows: "1fr 1fr",
            height: "100%", width: "100%"
        }
        const gridObj = {
            border: "5px solid black",
            overflowY: "scroll",
            padding: "5px"
        }

        let playerProjList = (this.state.playerState === this.state.enums.PlayerState.Player)  ?
            <div>
                <ProjectCurrent publicObjects={this.publicObjects} />
            </div>
            : "";

        let otherPlayerList = (this.state.playerState === this.state.enums.PlayerState.Player) ?
            <div>
                <OtherPlayerList publicObjects={this.publicObjects} />
            </div>
            : "";

        let upcomingProjList = (this.state.playerState === this.state.enums.PlayerState.Player) ?
            <div>
                <ProjectUpcoming publicObjects={this.publicObjects} />
            </div>
            : "";

        return (
            <div style={container} >
                <div style={infoBarSpace}>
                    <PlayerInfoBar publicObjects={this.publicObjects} />
                </div>
                <div style={activeSpace}>
                    <div style={mainSpace}>
                        <div style={grid}>
                            <div style={gridObj}>
                                <ExecutiveList publicObjects={this.publicObjects} />
                            </div>
                            <div style={gridObj}>
                                <b>Upcoming Projects:</b><br />
                                {upcomingProjList}
                            </div>
                            <div style={gridObj}>
                                <b>Your Projects:</b><br />
                                {playerProjList}
                            </div>
                            <div style={gridObj}>
                                <b>Other Players:</b><br />
                                {otherPlayerList}
                            </div>
                        </div>
                    </div>
                    <div style={logSpace}>
                        <div style={{ height: "50%", overflowY: "scroll" }}>
                            Current time: {(new Date(this.state.currentTime)).toLocaleTimeString()}
                            <Log items={this.state.gameLog.logEntries} />
                        </div>
                        <div style={{ height: "50%", overflowY: "scroll" }}>
                            <Chat publicObjects={this.publicObjects} />
                        </div>
                    </div>
                </div>
                <div style={handSpace}>
                    <CardHandMinimized publicObjects={this.publicObjects} />
                </div>
            </div>
        );
    }

    renderBidPhase() {
        const container = {
            width: "100vw",
            height: "100vh",
            display: "flex",
            flexDirection: "row",
            zIndex: 1
        }
        const mainSpace = {
            width: "85%",
            height: "100%",
            border: "5px solid black",
            display: "flex",
            flexDirection: "row"
        }
        const logSpace = {
            width: "15%",
            height: "100%",
            minWidth: "150px",
            display: "flex",
            flexDirection: "column",
            border: "5px solid black"
        }
        return (
            <div style={container}>
                <div style={mainSpace}>
                    <BidSubmission publicObjects={this.publicObjects} />
                    <ProjectUpcoming publicObjects={this.publicObjects} />
                </div>
                <div style={logSpace}>
                    <div style={{ height: "50%", overflowY: "scroll" }}>
                        Current time: {(new Date(this.state.currentTime)).toLocaleTimeString()}
                        <Log items={this.state.gameLog.logEntries} />
                    </div>
                    <div style={{ height: "50%", overflowY: "scroll" }}>
                        <Chat publicObjects={this.publicObjects} />
                    </div>
                </div>
            </div>
            );
    }
    
    render() {
        //If not authenticated, renders auth page.
        //If no game ID selected, renders game selection page.
        //Otherwise, renders the selected game.
        console.log("State:");
        console.log(this.state);
        let contents = null;

        if (this.state.isAuthenticated) {
            if (this.state.gameId === 0) {
                contents = this.renderGameSelectPage();
            }
            switch (this.state.gameState) {
                case this.state.enums.GameState.NotStarted:
                    contents = this.renderPreGamePhase();
                    break;
                case this.state.enums.GameState.Ended:
                case this.state.enums.GameState.WorkPhase:
                    contents = this.renderBuildPhase();
                    break;
                case this.state.enums.GameState.BidPhase:
                    contents = this.renderBidPhase();
                    break;
            }
        }
        else {
            contents = this.renderAuthPage();
        }

        let modal = this.renderModal();

        return (
            <div>
                <div>{contents}</div>
                <div>{modal}</div>
            </div>
        );
    }
}