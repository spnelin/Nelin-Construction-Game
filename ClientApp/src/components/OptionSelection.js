import React, { Component } from 'react';

export class OptionSelection extends Component {
    constructor(props) {
        super(props);
        this.invokeCallback = this.invokeCallback.bind(this);
    }

    invokeCallback(index, e) {
        this.props.selectCallback(index);
    }

    render() {
        return (
            <div>
                {this.props.title}
                <table>
                    <tbody>
                        {this.props.items.map((item, index) =>
                            <tr key={index}>
                                <td>{item.title}: </td>
                                <td>{item.text}</td>
                                <td><button onClick={(e) => this.invokeCallback(index, e)}>Select</button></td>
                            </tr>
                        )
                        }
                    </tbody>
                </table>
            </div>
        );
    }
}