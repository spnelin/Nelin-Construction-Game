import { ExpandableList } from "./ExpandableList";
import { ExecutiveAssignment, BuyMaterials, HireWorkers } from "./ExecutiveAssignment";
import React, { Component } from 'react';
import Utility from "./Utility";

export class ExecutiveInfo extends Component {
    constructor(props) {
        super(props);

        this.executive = props.executive;
    }

    render() {
        return (
            <div>
                {this.executive.name}
                <br />
                Quarterly Salary: {Utility.moneyString(this.executive.salary)}
                <br />
                {this.executive.text}
            </div>
        );
    }
}

export class ExecutiveList extends Component {
    constructor(props) {
        super(props);

        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;

        this.state = {
            ui: {

            },
            data: this.publicFunctions.getServerObject(this.constants.OBJECT_EXEC_INFO)
        };

        this.updateExecInfo = this.updateExecInfo.bind(this);
        this.executiveTask = this.executiveTask.bind(this);
        this.launchExecutiveAssignmentModal = this.launchExecutiveAssignmentModal.bind(this);
        this.renderExecutiveAssignmentModal = this.renderExecutiveAssignmentModal.bind(this);

        this.renderHeader = this.renderHeader.bind(this);
        this.renderContent = this.renderContent.bind(this);

        this.fireExecutiveConfirmation = this.fireExecutiveConfirmation.bind(this);
        this.fireExecutive = this.fireExecutive.bind(this);

        this.addListListener = this.addListListener.bind(this);

        this.publicFunctions.addListener(this.constants.OBJECT_EXEC_INFO, this.updateExecInfo);
    }

    componentWillUnmount() {
        this.publicFunctions.removeListener(this.constants.OBJECT_EXEC_INFO, this.updateExecInfo);
    }

    addListListener(listener) {
        this.listListener = listener;
    }

    updateExecInfo(data) {
        this.setState({ data: data });
        this.listListener(data.executives);
    }

    executiveTask(task, index) {
        switch (task) {
            case this.enums.ExecutiveTaskType.BuyMaterials:
            case this.enums.ExecutiveTaskType.HireWorkers:
                if (this.state.data.executives[index].currentTask === task) {
                    switch (task) {
                        case this.enums.ExecutiveTaskType.BuyMaterials:
                            this.publicFunctions.launchCustomModal(this.renderBuyMaterialsModal);
                            break;
                        case this.enums.ExecutiveTaskType.HireWorkers:
                            this.publicFunctions.launchCustomModal(this.renderHireWorkModal);
                            break;
                    }
                    break;
                }
            //no break - if not the current task, continue to standard ExecutiveAssignment
            case this.enums.ExecutiveTaskType.HireExecutive:
            case this.enums.ExecutiveTaskType.FindOpportunity:
                this.launchExecutiveAssignmentModal(index, this.getTaskIndex(task));
                break;
        }
    }

    getTaskIndex(type) {
        for (let index = 0; index < this.state.data.taskList.length; index++) {
            if (this.state.data.taskList[index].type === type) {
                return index;
            }
        }
        return -1;
    }

    taskDisabled(task, exec) {
        switch (task) {
            case this.enums.ExecutiveTaskType.BuyMaterials:
            case this.enums.ExecutiveTaskType.HireWorkers:
                if (exec.currentTask === task) {
                    return false;
                }
            //no break - if not found, continue to next option
            case this.enums.ExecutiveTaskType.HireExecutive:
            case this.enums.ExecutiveTaskType.FindOpportunity:
                return !exec.actionReady;
        }
    }

    fireExecutiveConfirmation(index) {
        this.publicFunctions.launchConfirmationModal("Are you sure you want to fire this executive?", () => this.fireExecutive(index)); 
    }

    fireExecutive(index) {
        this.publicFunctions.post("FireExecutive", { executiveIndex: index });
    }

    launchExecutiveAssignmentModal(execIndex, taskIndex) {
        this.publicFunctions.launchCustomModal(() => this.renderExecutiveAssignmentModal(execIndex, taskIndex));
    }

    renderExecutiveAssignmentModal(execIndex, taskIndex) {
        return (<ExecutiveAssignment execIndex={execIndex} taskIndex={taskIndex} publicObjects={this.publicFunctions.getPublicObjects()} />);
    }

    renderBuyMaterialsModal(siteCore) {
        return (
            <div>
                <BuyMaterials publicObjects={siteCore.publicObjects} />
            </div>
        );
    }

    renderHireWorkModal(siteCore) {
        return (
            <div>
                <HireWorkers publicObjects={siteCore.publicObjects} />
            </div>
        );
    }

    renderHeader(exec) {
        console.log("Rendering header")
        let readySymbol = exec.actionReady ? "[O]" : "[X]";
        return (<div id="header">{readySymbol}{exec.name}</div>);
    }

    renderContent(exec, index) {
        let fireButton = index == 0 ? "" : <button onClick={() => this.fireExecutiveConfirmation(index)}>Fire</button>;

        return (
            <div>
                Quarterly Salary: {Utility.moneyString(exec.salary)}<br />
                {exec.text}<br />
                <button onClick={() => this.executiveTask(this.enums.ExecutiveTaskType.BuyMaterials, index)} disabled={this.taskDisabled(this.enums.ExecutiveTaskType.BuyMaterials, exec)}>Buy Materials</button>
                <button onClick={() => this.executiveTask(this.enums.ExecutiveTaskType.HireWorkers, index)} disabled={this.taskDisabled(this.enums.ExecutiveTaskType.HireWorkers, exec)}>Hire Workers</button><br />
                <button onClick={() => this.executiveTask(this.enums.ExecutiveTaskType.HireExecutive, index)} disabled={this.taskDisabled(this.enums.ExecutiveTaskType.HireExecutive, exec)}>Hire Executive</button>
                <button onClick={() => this.executiveTask(this.enums.ExecutiveTaskType.FindOpportunity, index)} disabled={this.taskDisabled(this.enums.ExecutiveTaskType.FindOpportunity, exec)}>Find Opportunity</button><br />
                {fireButton}
            </div>
        );
    }

    render() {
        console.log("rerendering executivelist")
        return this.state.data === null || this.state.data.executives.length === 0 ? "" :
            <ExpandableList
                title={<span>Your Executives: <button onClick={() => this.launchExecutiveAssignmentModal(null, this.getTaskIndex(this.enums.ExecutiveTaskType.HireExecutive))}>Hire Executive</button></span>}
                items={this.state.data.executives}
                headerRenderer={this.renderHeader}
                contentRenderer={this.renderContent}
                itemListener={this.addListListener}
            />;
    }
}