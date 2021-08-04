import React, { Component } from 'react';
import { Log } from './Log';
import { Modal, ModalSubmitCancel } from './Modal';

export class Chat extends Component {
    constructor(props) {
        super(props);

        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;

        let chatObj = this.publicFunctions.getServerObject(this.constants.OBJECT_CHAT);
        let timestamps = [];

        for (let i = 0; i < chatObj.chats.length; i++) {
            timestamps[i] = chatObj.chats[i].log.logEntries[chatObj.chats[i].log.logEntries.length - 1].timestamp;
        }

        this.state = {
            data: chatObj,
            otherPlayerObj: this.publicFunctions.getServerObject(this.constants.OBJECT_OTHER_PLAYERS),
            ui: {
                typedMessage: "",
                currentChat: 0,
                latestCheckedArr: timestamps,
                participantsExpanded: false
            }
        };

        this.updateChat = this.updateChat.bind(this);
        this.updateOtherPlayerObj = this.updateOtherPlayerObj.bind(this);
        this.onChangeChatSelection = this.onChangeChatSelection.bind(this);
        this.onChangeChatVal = this.onChangeChatVal.bind(this);
        this.onChatKeyDown = this.onChatKeyDown.bind(this);
        this.onClickChatCreateOpen = this.onClickChatCreateOpen.bind(this);
        this.onClickChatParticipants = this.onClickChatParticipants.bind(this);

        //subscribe to chatObj and otherPlayerObj
        this.publicFunctions.addListener(this.constants.OBJECT_CHAT, this.updateChat);
        this.publicFunctions.addListener(this.constants.OBJECT_OTHER_PLAYERS, this.updateOtherPlayerObj);
    }

    componentWillUnmount() {
        this.publicFunctions.removeListener(this.constants.OBJECT_CHAT, this.updateChat);
        this.publicFunctions.removeListener(this.constants.OBJECT_OTHER_PLAYERS, this.updateOtherPlayerObj);
    }

    updateChat(data) {
        let timestamps = this.state.ui.latestCheckedArr;
        let chat = data.chats[this.state.ui.currentChat];
        timestamps[this.state.ui.currentChat] = chat.log.logEntries[chat.log.logEntries.length - 1].timestamp;
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                latestCheckedArr: timestamps
            },
            data: data
        }));
    }

    updateOtherPlayerObj(data) {
        this.setState({ otherPlayerObj: data });
    }

    onChangeChatSelection(e) {
        let val = e.target.value;
        let timestamps = this.state.ui.latestCheckedArr;
        timestamps[val] = this.state.data.chats[val].log.logEntries[this.state.data.chats[val].log.logEntries.length - 1].timestamp;
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                currentChat: val,
                latestCheckedArr: timestamps,
                typedMessage: ""
            }
        }));
    }

    onChangeChatVal(e) {
        //preserve value
        let val = e.target.value;
        if (val.length > 1000) {
            val = val.substring(0, 999);
        }
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                typedMessage: val
            }
        }));
    }

    onChatKeyDown(e) {
        if (e.key === 'Enter') {
            this.publicFunctions.gameActionFetch(`EnterChatLine?chatId=${this.state.data.chats[this.state.ui.currentChat].id}&entry=${encodeURIComponent(this.state.ui.typedMessage)}`);
            this.setState(prevState => ({
                ui: {
                    ...prevState.ui,
                    typedMessage: ""
                }
            }));
        }
    }

    onClickChatCreateOpen(e) {
        e.preventDefault();
        this.publicFunctions.launchSubmitModal(() => this.renderChatCreateModal(this.publicFunctions.getPublicObjects()));
    }

    onClickChatParticipants(e) {
        e.preventDefault();
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                participantsExpanded: !prevState.ui.participantsExpanded
            }
        }));
    }

    getShortParticipantsList() {
        let ret = "";
        let players = this.state.data.chats[this.state.ui.currentChat].players;
        for (let i = 0; i < players.length; i++) {
            if (ret !== "") {
                ret += ", ";
            }
            ret += players[i].name;
            if (ret.length > 30 && i + 1 < players.length) {
                ret += "..."
                break;
            }
        }
        return ret;
    }

    disableNewChat() {
        return !(this.state.otherPlayerObj && this.state.otherPlayerObj.length > 0);
    }

    writeChatName(chat, index) {
        let name = chat.log.logEntries[chat.log.logEntries.length - 1].timestamp === this.state.ui.latestCheckedArr[index] ? "" : "* ";
        name += chat.name;
        return name;
    }

    renderChatSelector() {
        let highlight = {};
        for (let chatIndex = 0; chatIndex < this.state.data.chats.length; chatIndex++) {
            let chat = this.state.data.chats[chatIndex];
            if (chat.log.logEntries[chat.log.logEntries.length - 1].timestamp !== this.state.ui.latestCheckedArr[chatIndex]) {
                highlight = { background: "rgba(200, 0, 0, 0.4)" };
            }
        }
        return (
            <select value={this.state.ui.currentChat}
                style={highlight} 
                onChange={this.onChangeChatSelection}>
                {this.state.data.chats.map((chat, index) =>
                    <option key={index} value={index}>{this.writeChatName(chat, index)}</option>
                )}
            </select>
        );
    }

    renderChatCreateModal(publicObjects) {
        return (
            <ChatCreator publicObjects={publicObjects} />
        );
    }

    renderNewChatButton() {
        return (
            <button onClick={this.onClickChatCreateOpen} disabled={this.disableNewChat()}>New Chat</button>
        );
    }

    renderChatParticipants() {
        return this.state.ui.participantsExpanded ? (
            <div>
                <button onClick={this.onClickChatParticipants}>^</button>
                {this.state.data.chats[this.state.ui.currentChat].players.map((player, index) =>
                    <div key={index}>
                        <span style={{ display: "inline-block" }}>
                            {player.name}
                        </span>
                        <br />
                    </div>
                )}
            </div>
            ) : (
                <div>
                    <button onClick={this.onClickChatParticipants}>v</button>
                    <span style={{ display: "inline-block", fontStyle: "italic" }}>
                        {this.getShortParticipantsList()}
                    </span>
                    <br />
                </div>
            );

    }

    renderChatLog() {
        return (
            <Log items={this.state.data.chats[this.state.ui.currentChat].log.logEntries} />
        );
    }

    renderChatEntry() {
        return (
            <input type="text" value={this.state.ui.typedMessage} onChange={this.onChangeChatVal} onKeyDown={this.onChatKeyDown} />
            );
    }
    
    render() {
        return (
            <div>
                {this.renderChatSelector()}
                {this.renderNewChatButton()}
                {this.renderChatParticipants()}
                {this.renderChatLog()}
                {this.renderChatEntry()}
            </div>
        );
    }
}

export class ChatCreator extends Component {
    constructor(props) {
        super(props);
        
        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;
        this.publicRenders = props.publicObjects.renders;

        this.state = {
            chatObj: this.publicFunctions.getServerObject(this.constants.OBJECT_CHAT),
            otherPlayerObj: this.publicFunctions.getServerObject(this.constants.OBJECT_OTHER_PLAYERS),
            ui: {
                chatName: "",
                playerCheckArr: []
            }
        };

        this.updateChat = this.updateChat.bind(this);
        this.updateOtherPlayerObj = this.updateOtherPlayerObj.bind(this);
        this.onChangeChatNameVal = this.onChangeChatNameVal.bind(this);
        this.onCheckPlayerBox = this.onCheckPlayerBox.bind(this);
        this.submitChatCreate = this.submitChatCreate.bind(this);
        this.chatCreatorSubmitDisabled = this.chatCreatorSubmitDisabled.bind(this);

        //subscribe to chatObj and otherPlayerObj
        this.publicFunctions.addListener(this.constants.OBJECT_CHAT, this.updateChat);
        this.publicFunctions.addListener(this.constants.OBJECT_OTHER_PLAYERS, this.updateOtherPlayerObj);

        //register the modal handler
        this.publicFunctions.addModalSubmitHandler(this.submitChatCreate);
    }

    componentWillUnmount() {
        this.publicFunctions.removeListener(this.constants.OBJECT_CHAT, this.updateChat);
        this.publicFunctions.removeListener(this.constants.OBJECT_OTHER_PLAYERS, this.updateOtherPlayerObj);
    }

    updateChat(data) {
        this.setState({ chatObj: data });
    }

    updateOtherPlayerObj(data) {
        this.setState({ otherPlayerObj: data });
    }

    onChangeChatNameVal(e) {
        let val = e.target.value;
        if (val.length > 1000) {
            val = val.substring(0, 999);
        }
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                chatName: val
            }
        }));
    }

    onCheckPlayerBox(e, index) {
        let arr = this.state.ui.playerCheckArr;
        arr[index] = e.target.checked;
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                playerCheckArr: arr
            }
        }));
    }

    chatCreatorSubmitDisabled() {
        let arr = this.state.ui.playerCheckArr;
        let anyChecked = false;
        let idArr = [];
        for (let index in arr) {
            if (arr[index]) {
                anyChecked = true;
                idArr.push(this.state.otherPlayerObj[index].id);
            }
        }
        let isUnique = true;
        for (let chat in this.state.chatObj) {
            let idCt = 0;
            for (let player in chat.players) {
                if (idArr.find(i => i === player.id)) {
                    idCt++;
                }
                else {
                    idCt = -1;
                    break;
                }
            }
            if (idCt === idArr.length) {
                isUnique = false;
            }
        }
        return (this.state.ui.chatName === "") || !anyChecked || !isUnique;
    }

    submitChatCreate() {
        let arr = this.state.ui.playerCheckArr;
        let ids = ""
        for (let index in arr) {
            if (arr[index]) {
                if (ids === "") {
                    ids += this.state.otherPlayerObj[index].id;
                }
                else {
                    ids += "," + this.state.otherPlayerObj[index].id;
                }
            }
        }
        this.publicFunctions.gameActionFetch(`CreateChat?name=${encodeURIComponent(this.state.ui.chatName)}&playerIds=${ids}`);
    }

    render() {
        return (
            <Modal>
                Chat name:
                <input type="text" value={this.state.ui.chatName} onChange={this.onChangeChatNameVal} />
                Check the players you want included:
                {this.state.otherPlayerObj.map((player, index) =>
                    <label key={index}>
                        {player.name}:
                        <input type="checkbox" onClick={(e) => { this.onCheckPlayerBox(e, index) }} />
                    </label>
                )}
                <ModalSubmitCancel onSubmit={this.publicFunctions.submitModal} onCancel={this.publicFunctions.cancelModal} submitDisabled={this.chatCreatorSubmitDisabled()} />
            </Modal>
        );
    }
}