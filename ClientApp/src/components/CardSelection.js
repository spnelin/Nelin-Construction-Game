import React, { Component } from 'react';
import '../styles/card-selection.css'

export class CardSelection extends Component {
    constructor(props) {
        super(props);

        this.title = props.title ? props.title : "Select an option:";
        this.cards = props.cards;
        this.cardRenderer = props.cardRenderer;
        this.selectionHandler = props.selectionHandler;
        this.selectionDisabled = props.selectionDisabled;
        this.selectionText = props.selectionText ? props.selectionText : "Select";
        this.cancelHandler = props.cancelHandler;
        this.cancelText = props.cancelText ? props.cancelText : "Cancel";

        this.select = this.select.bind(this);
        this.cancel = this.cancel.bind(this);
    }

    select(e, card, index) {
        e.preventDefault();
        this.selectionHandler(card, index);
    }

    cancel(e) {
        e.preventDefault();
        this.cancelHandler();
    }

    renderCancel() {
        return this.cancelHandler ? (<button onClick={this.cancel}>{this.cancelText}</button>) : "";
    }

    render() {
        console.log("Cards:")
        console.log(this.cards)
        return (
            <div>
                <span>{this.title}</span>
                <div className="card-selection-grid">
                    {this.cards.map((card, index) =>
                        <div className="card-selection-box" key={index}>
                            <div className="card-selection-content">{this.cardRenderer(card, index)}</div>
                            <br />
                            <button onClick={(e) => this.select(e, card, index)} disabled={this.selectionDisabled(card, index)}>{this.selectionText}</button>
                        </div>)}
                </div>
                {this.renderCancel()}
            </div>
        );
    }
}