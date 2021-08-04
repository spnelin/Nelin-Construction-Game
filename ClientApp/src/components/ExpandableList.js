import React, { Component } from 'react';
import '../styles/expandable-list.css'

export class ExpandableList extends Component {
    constructor(props) {
        super(props);

        this.title = props.title;
        this.headerRenderer = props.headerRenderer;
        this.contentRenderer = props.contentRenderer;

        this.state = {
            items: props.items,
            expandedIndex: -1
        };

        this.expand = this.expand.bind(this);
        this.collapse = this.collapse.bind(this);
        this.renderItem = this.renderItem.bind(this);
        this.updateItems = this.updateItems.bind(this);

        if (props.itemListener) {
            console.log("Adding item listener")
            props.itemListener(this.updateItems);
        }
    }

    updateItems(items) {
        console.log("Updating items")
        let newIndex = items.length < this.state.items.length ? -1 : this.state.expandedIndex;
        this.setState({ items: items, expandedIndex: newIndex });
    }

    expand(e, index) {
        e.preventDefault();
        this.setState({ expandedIndex: index });
    }

    collapse(e) {
        e.preventDefault();
        this.setState({ expandedIndex: -1 });
    }

    renderItem(item, index) {
        console.log("Rendering item")
        console.log(item)
        console.log(index)
        return index === this.state.expandedIndex ?
            (<div className="expandable-list-box" key={index}>
                <div className="expandable-list-header" onClick={this.collapse}>{this.headerRenderer(item, index)}</div>
                <br />
                <div className="expandable-list-content">{this.contentRenderer(item, index)}</div>
            </div>) : (<div className="expandable-list-box" key={index}>
                <div className="expandable-list-header" onClick={(e) => this.expand(e, index)}>{this.headerRenderer(item, index)}</div>
            </div>);
    }

    render() {
        console.log("rerendering list")
        console.log(this.state.items)
        return (
            <div>
                <span>{this.title}</span>
                <div className="expandable-list-grid">
                    {this.state.items.map((item, index) => this.renderItem(item, index))}
                </div>
            </div>
        );
    }
}