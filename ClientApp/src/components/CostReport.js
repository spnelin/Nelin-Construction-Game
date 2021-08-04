import React, { Component } from 'react';
import Utility from './Utility';

export class CostReport extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                Worker salaries: {Utility.moneyString(this.props.modalObj.data.workerSalary)}
                <br />
                Executive salaries: {Utility.moneyString(this.props.modalObj.data.executiveSalary)}
            </div>
        );
    }
}