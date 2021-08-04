import React, { Component } from 'react';
import '../styles/modal-styles.css';

export class Modal extends Component {
    constructor(props) {
        super(props);
        //todo: allow passing PublicObjects in, so we can just fetch the handlers directly from sitecore
    }

    renderButtons() {
        let modalType = this.props.modalType ? this.props.modalType.toUpperCase() : "";
        switch (modalType) {
            case "SUBMITCANCEL":
                return (
                    <ModalSubmitCancel onSubmit={this.props.onSubmit} submitDisabled={this.props.submitDisabled} onCancel={this.props.onCancel} cancelDisabled={this.props.cancelDisabled} />
                );
            case "OK":
                return (
                    <ModalOk onSubmit={this.props.onSubmit} />
                );
            default:
                return "";
        }
    }

    render() {
        console.log("Rendering new modal")
        return (
            <div className="site-modal">
                <div className="close">x</div>
                <div className="site-modal-content">
                    <div className="site-modal-text">
                        {this.props.children}
                    </div>
                    {this.renderButtons()}
                </div>
            </div>
        );
    }
}

export class ModalSubmitCancel extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                <div><button className="button1" onClick={this.props.onSubmit} disabled={this.props.submitDisabled}>Submit</button></div>
                <div><button className="button2" onClick={this.props.onCancel} disabled={this.props.cancelDisabled}>Cancel</button></div>
            </div>
        );
    }
}

export class ModalOk extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                <div><button className="button1" onClick={this.props.onSubmit}>Ok</button></div>
            </div>
        );
    }
}