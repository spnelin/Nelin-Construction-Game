import React, { Component } from 'react';

export class Log extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                {this.props.items.map((item, index) =>
                    <div key={index}>
                        <span style={{ display: "inline-block" }}>
                            {(new Date(item.timestamp)).toLocaleTimeString()}: {item.text}
                        </span>
                        <br />
                    </div>
                    )
                }
            </div>
        );
    }
}