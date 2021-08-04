import React, { Component } from 'react';
import Utility from './Utility';
import { Modal } from './Modal';
import { ExpandableList } from './ExpandableList';

export class ProjectStaticDisplay extends Component {
    constructor(props) {
        super(props);
    }

    //warning: hardcoded for now
    typeName(type) {
        switch (type) {
            case 0:
                return "Building";
            case 1:
                return "Bridge";
            case 2:
                return "Tunnel";
        }
    }
    tierName(tier) {
        switch (tier) {
            case 0:
                return "Tier 1";
            case 1:
                return "Tier 2";
            case 2:
                return "Tier 3";
        }
    }

    renderReqs() {
        return this.props.projData.requirements.length == 0 ? "" : (
            <table>
                <thead>
                    <tr key="a">
                        <th>Prerequisites:</th>
                    </tr>
                </thead>
                <tbody>
                    <tr key="b">
                        <td>{this.props.projData.canBid ? <span style={{ color: "green" }}>You meet all requirements and can bid on this project.</span> : <span style={{ color: "red" }}>You do not meet all requirements and cannot bid on this project.</span>}</td>
                    </tr>
                    {this.props.projData.requirements.map((req, index) =>
                        <tr key={index}>
                            <td>Completed {req.quantity} {this.tierName(req.tier)} {this.typeName(req.type)}</td>
                        </tr>
                    )
                    }
                </tbody>
            </table>
        );
    }

    render() {
        let concreteQuant = "";
        if (this.props.projData.totalConcrete > 0) {
            concreteQuant = "Concrete: ";
            if (this.props.inProgress) {
                concreteQuant = concreteQuant + this.props.projData.currentConcrete + "/";
            }
            concreteQuant = concreteQuant + this.props.projData.totalConcrete;
            if (this.props.addingMaterials) {
                concreteQuant = concreteQuant + " (+" + this.props.tentativeQuantities[0] + ")"; //yup more hardcoding
            }
            concreteQuant = concreteQuant + "\n";
        }
        let steelQuant = "";
        if (this.props.projData.totalSteel > 0) {
            steelQuant = "Steel: ";
            if (this.props.inProgress) {
                steelQuant = steelQuant + this.props.projData.currentSteel + "/";
            }
            steelQuant = steelQuant + this.props.projData.totalSteel;
            if (this.props.addingMaterials) {
                steelQuant = steelQuant + " (+" + this.props.tentativeQuantities[1] + ")"; //yup more hardcoding
            }
            steelQuant = steelQuant + "\n";
        }
        let glassQuant = "";
        if (this.props.projData.totalGlass > 0) {
            glassQuant = "Glass: ";
            if (this.props.inProgress) {
                glassQuant = glassQuant + this.props.projData.currentGlass + "/";
            }
            glassQuant = glassQuant + this.props.projData.totalGlass;
            if (this.props.addingMaterials) {
                glassQuant = glassQuant + " (+" + this.props.tentativeQuantities[2] + ")"; //yup more hardcoding
            }
            glassQuant = glassQuant + "\n";
        }
        let workers = "Maximum workers per turn: "
        if (this.props.inProgress) {
            workers = workers + this.props.projData.workersThisTurn + "/";
        }
        workers = workers + this.props.projData.maxWorkers;
        if (this.props.addingMaterials) {
            workers = workers + " (+" + this.props.tentativeWorkers + ")";
        }
        let requirements = "";
        if (!this.props.inProgress) {
            requirements = this.renderReqs();
        }
        let maxBid = this.props.inProgress ? "" : `Maximum bid: ${Utility.moneyString(this.props.projData.maxBid)}`;
        let maxBidBreak = "";
        if (maxBid !== "") { maxBidBreak = <br /> }
        return (
            <div>
                {this.props.excludeName ? "" : this.props.projData.name}{this.props.excludeName ? "" : <br />}
                {this.props.projData.text}<br />
                Due by: {Utility.turnNameFromNumber(this.props.projData.turnDueBy)}<br />
                {maxBid}{maxBidBreak}
                {this.tierName(this.props.projData.tier)} {this.typeName(this.props.projData.type)}<br />
                {concreteQuant + steelQuant + glassQuant + workers}<br />
                {requirements}
            </div>
        );
    }
}

export class ProjectCurrent extends Component {
    constructor(props) {
        super(props);

        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;

        this.state = {
            currProjects: this.publicFunctions.getServerObject(this.constants.OBJECT_CURRENT_PLAYER).currentProjects,
            compProjects: this.publicFunctions.getServerObject(this.constants.OBJECT_CURRENT_PLAYER).completedProjects
        };

        this.renderButton = this.renderButton.bind(this);
        this.onSelectListProj = this.onSelectListProj.bind(this);
        this.updateProjects = this.updateProjects.bind(this);
        this.renderProjWorkModal = this.renderProjWorkModal.bind(this);
        this.onClickViewCompProj = this.onClickViewCompProj.bind(this);
        this.renderCompProjListModal = this.renderCompProjListModal.bind(this);

        this.publicFunctions.addListener(this.constants.OBJECT_CURRENT_PLAYER, this.updateProjects)

        this.projectListListener = (a) => "";
        this.addUpcomingProjectsListener = this.addUpcomingProjectsListener.bind(this);
    }

    componentWillUnmount() {
        this.publicFunctions.removeListener(this.constants.OBJECT_CURRENT_PLAYER, this.updateProjects);
    }

    updateProjects(data) {
        this.setState({
            currProjects: data.currentProjects,
            compProjects: data.completedProjects
        });
        this.projectListListener(data.currentProjects);
    }

    addUpcomingProjectsListener(listener) {
        this.projectListListener = listener;
    }

    onSelectListProj(e, index) {
        e.preventDefault();
        this.publicFunctions.launchCustomModal((siteCore) => this.renderProjWorkModal(siteCore, index));
    }

    onClickViewCompProj(e) {
        e.preventDefault();
        this.publicFunctions.launchOkModal(this.renderCompProjListModal);
    }

    renderProjWorkModal(siteCore, index) {
        return (
            <ProjectWorkInterface publicObjects={siteCore.publicObjects} projIndex={index} />
        );
    }

    renderButton(proj, index) {
        return (
            <div>
                <button onClick={(e) => this.onSelectListProj(e, index)}>Construct</button>
            </div>
        );
    }

    renderCompProjListModal() {
        return (
            <div>
                <ProjectList projects={this.state.compProjects} inProgress="false" />
            </div>
        );
    }

    render() {
        let currProjects = this.state.currProjects && this.state.currProjects.length > 0 ? (
            <ProjectList title="Current Projects:"
                projects={this.state.currProjects}
                itemListener={this.addUpcomingProjectsListener}
                actionRenderer={this.renderButton}
                inProgress={true}
            />
        ) : "";
        let compProjects = this.state.compProjects && this.state.compProjects.length === 0 ? "" :
            <button onClick={this.onClickViewCompProj}>Completed Projects</button>;
        return (
            <div>
                {compProjects}<br />
                {currProjects}
            </div>
        );
    }
}

export class ProjectWorkInterface extends Component {
    constructor(props) {
        super(props);

        this.projIndex = props.projIndex;

        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;

        let playerInfo = this.publicFunctions.getServerObject(this.constants.OBJECT_CURRENT_PLAYER);

        let project = this.publicFunctions.getServerObject(this.constants.OBJECT_CURRENT_PROJECTS)[this.projIndex];

        this.state = {
            ui: {
                quantities: [0, 0, 0],
                //note: initialize to actual server data on free workers, but then modify on client side. This will be reloaded when we reload the component, and not sooner - got to preserve client modifications.
                freeWorkers: playerInfo.freeWorkers,
                workersThisTurn: project.workersThisTurn,
                warningMessage: ""
            },
            data: project,
            playerInfo: playerInfo
        };

        let requiredExecAbility = this.enums.ExecutiveAbility.None;
        this.betterBuilding = false;
        switch (project.type) {
            case this.enums.ProjectType.Bridge:
                requiredExecAbility = this.enums.ExecutiveAbility.BetterBridgeBuilding;
                break;
            case this.enums.ProjectType.Building:
                requiredExecAbility = this.enums.ExecutiveAbility.BetterBuildingBuilding;
                break;
            case this.enums.ProjectType.Tunnel:
                requiredExecAbility = this.enums.ExecutiveAbility.BetterTunnelBuilding;
                break;
        }
        if (requiredExecAbility !== this.enums.ExecutiveAbility.None) {
            console.log("checking for better building")
            let execs = this.publicFunctions.getServerObject(this.constants.OBJECT_EXEC_INFO).executives;
            for (let i = 0; i < execs.length; i++) {
                if (execs[i].ability === requiredExecAbility) {
                    this.betterBuilding = true;
                    console.log("enabling better building")
                    break;
                }
            }
        }

        this.onSubmitProjWork = this.onSubmitProjWork.bind(this);
        this.submitDisabled = this.submitDisabled.bind(this);
        this.onChangeProjResQuant = this.onChangeProjResQuant.bind(this);
        this.onDecProjResQuant = this.onDecProjResQuant.bind(this);
        this.onIncProjResQuant = this.onIncProjResQuant.bind(this);
        this.renderMaterialSelect = this.renderMaterialSelect.bind(this);

        this.publicFunctions.addModalSubmitHandler(this.onSubmitProjWork);
    }

    onSubmitProjWork() {
        this.publicFunctions.gameActionFetch(`ConstructProject?projIndex=${this.projIndex}&concrete=${this.state.ui.quantities[0]}&steel=${this.state.ui.quantities[1]}&glass=${this.state.ui.quantities[2]}`);
    }

    submitDisabled() {
        console.log("Material quantities:")
        console.log(this.state.ui.quantities)
        console.log(this.state.ui.quantities[0] + this.state.ui.quantities[1] + this.state.ui.quantities[2] < 1)
        return this.state.ui.quantities[0] + this.state.ui.quantities[1] + this.state.ui.quantities[2] < 1;
    }

    onChangeProjResQuant(e, material) {
        let quant = Number(e.target.value);
        if (isNaN(quant)) {
            return;
        }
        this.updateToProjResQuant(material, quant);
    }

    onDecProjResQuant(e, material) {
        e.preventDefault();
        let quant = this.state.ui.quantities[material];
        quant--;
        this.updateToProjResQuant(material, quant);
    }

    onIncProjResQuant(e, material) {
        e.preventDefault();
        let quant = this.state.ui.quantities[material];
        quant++;
        this.updateToProjResQuant(material, quant);
    }

    materialHeld(material) {
        switch (material) {
            case this.enums.MaterialType.Concrete:
                return this.state.playerInfo.concrete;
            case this.enums.MaterialType.Steel:
                return this.state.playerInfo.steel;
            case this.enums.MaterialType.Glass:
                return this.state.playerInfo.glass;
        }
    }

    materialNeeded(material) {
        switch (material) {
            case this.enums.MaterialType.Concrete:
                return this.state.data.totalConcrete - this.state.data.currentConcrete;
            case this.enums.MaterialType.Steel:
                return this.state.data.totalSteel - this.state.data.currentSteel;
            case this.enums.MaterialType.Glass:
                return this.state.data.totalGlass - this.state.data.currentGlass;
        }
    }

    updateToProjResQuant(material, quant) {
        if (quant < 0) {
            quant = 0;
        }
        let warnText = "";
        let abort = false;
        let resNeeded = this.materialNeeded(material);
        //If this material is "full" we can reset quant down to a valid value - disallow overspending
        if (quant > resNeeded) {
            quant = resNeeded;
            warnText = "You already have enough of this material assigned.";
        }
        let quantities = this.state.ui.quantities;
        let oldQuantity = quantities[material];
        quantities[material] = quant;
        let resourcesThisBuild = quantities[0] + quantities[1] + quantities[2];
        let workersThisBuild = this.betterBuilding ? Math.ceil(resourcesThisBuild / 2) : resourcesThisBuild;
        let potentialFreeWorkers = this.state.playerInfo.freeWorkers - workersThisBuild;
        if (potentialFreeWorkers < 0) {
            warnText = "You have no more workers to assign to this project.";
            abort = true;
        }
        let potentialWorkersThisTurn = this.state.data.workersThisTurn + workersThisBuild;
        if (potentialWorkersThisTurn > this.state.data.maxWorkers) {
            warnText = "You can't assign any more workers to this project this turn.";
            abort = true;
        }
        //If the total material cost in question is greater than what we have in stock
        if (this.materialHeld(material) < quant) {
            if (warnText !== "") {
                warnText = warnText + "\n";
            }
            warnText = warnText + "You do not have enough materials.";
            abort = true;
        }
        if (abort) {
            //reset temp values so we don't change anything
            quantities[material] = oldQuantity;
            potentialFreeWorkers = this.state.ui.freeWorkers;
            potentialWorkersThisTurn = this.state.ui.workersThisTurn;
        }
        this.setState(prevState => ({
            ui: {
                ...prevState.ui,
                quantities: quantities,
                freeWorkers: potentialFreeWorkers,
                workersThisTurn: potentialWorkersThisTurn,
                warningMessage: warnText
            }
        }));
    }

    materialName(material) {
        switch (material) {
            case this.enums.MaterialType.Concrete:
                return "Concrete";
            case this.enums.MaterialType.Steel:
                return "Steel";
            case this.enums.MaterialType.Glass:
                return "Glass";
        }
    }

    renderMaterialSelect(material) {
        return (
            <div>
                <br />
                {this.materialName(material)}:
                <button onClick={(e) => this.onDecProjResQuant(e, material)}>-</button>
                <input type="text" pattern="[0-9]*"
                    value={this.state.ui.quantities[material]}
                    onChange={(e) => this.onChangeProjResQuant(e, material)} />
                <button onClick={(e) => this.onIncProjResQuant(e, material)}>+</button>
                You currently have: {this.materialHeld(material)}
            </div>
        );
    }

    render() {
        let concreteSelect = this.state.data.totalConcrete > 0 ? this.renderMaterialSelect(this.enums.MaterialType.Concrete) : "";
        let steelSelect = this.state.data.totalSteel > 0 ? this.renderMaterialSelect(this.enums.MaterialType.Steel) : "";
        let glassSelect = this.state.data.totalGlass > 0 ? this.renderMaterialSelect(this.enums.MaterialType.Glass) : "";
        return (
            <Modal modalType="SUBMITCANCEL" onSubmit={this.publicFunctions.submitModal} onCancel={this.publicFunctions.cancelModal} submitDisabled={this.submitDisabled()} >
                <ProjectStaticDisplay projData={this.state.data} inProgress={true} addingMaterials={true} tentativeQuantities={this.state.ui.quantities} tentativeWorkers={this.state.ui.workersThisTurn - this.state.data.workersThisTurn} />
                <br />
                <div>
                    {this.state.ui.warningMessage}
                    {concreteSelect}
                    {steelSelect}
                    {glassSelect}
                    <br />
                    Free workers: {this.state.ui.freeWorkers}/{this.state.playerInfo.totalWorkers}
                    Maximum workers: {this.state.ui.workersThisTurn}/{this.state.data.maxWorkers}
                </div>
            </Modal>
        );
    }
}

export class ProjectPipeline extends Component {
    constructor(props) {
        super(props);

        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;

        let gameInfo = this.publicFunctions.getServerObject(this.constants.OBJECT_GAME);
        let turnArr = [];
        for (let i = 0; i < 6; i++) {
            turnArr[i] = Utility.turnNameFromNumber(gameInfo.turnNumber + i);
        }
        this.state = {
            data: this.publicFunctions.getServerObject(this.constants.OBJECT_PROJECT_PIPELINE),
            turnNames: turnArr
        };

        this.updateUpcomingProjects = this.updateUpcomingProjects.bind(this);
        this.updateGameTurns = this.updateGameTurns.bind(this);

        this.publicFunctions.addListener(this.constants.OBJECT_PROJECT_PIPELINE, this.updateUpcomingProjects);
        this.publicFunctions.addListener(this.constants.OBJECT_GAME, this.updateGameTurn);
    }

    componentWillUnmount() {
        this.publicFunctions.removeListener(this.constants.OBJECT_PROJECT_PIPELINE, this.updateUpcomingProjects);
        this.publicFunctions.removeListener(this.constants.OBJECT_GAME, this.updateGameTurn);
    }

    updateUpcomingProjects(data) {
        this.setState({ data: data });
    }

    updateGameTurns(data) {
        let turnArr = [];
        for (let i = 0; i < 6; i++) {
            turnArr[i] = Utility.turnNameFromNumber(data.turnNumber + i);
        }
        this.setState({ turnNames: turnArr });;
    }

    renderProjDisplay(proj) {
        if (proj === null) {
            return ("");
        }
        else {
            return (<ProjectStaticDisplay projData={proj} inProgress={false} />);
        }
    }

    render() {
        return (
            <table>
                <thead>
                    <tr>
                        <th>Available turn for bidding:</th>
                        {this.state.turnNames.map((turn, index) => <th key={index}>{turn}</th>)}
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            Tier 3 projects:
                        </td>
                        {this.state.data.tier3Projects.map((proj, index) => <td key={index}>{this.renderProjDisplay(proj)}</td>)}
                    </tr>
                    <tr>
                        <td>
                            Tier 2 projects:
                        </td>
                        {this.state.data.tier2Projects.map((proj, index) => <td key={index}>{this.renderProjDisplay(proj)}</td>)}
                    </tr>
                    <tr>
                        <td>
                            Tier 1 projects:
                        </td>
                        {this.state.data.tier1Projects.map((proj, index) => <td key={index}>{this.renderProjDisplay(proj)}</td>)}
                    </tr>
                </tbody>
            </table>);
    }
}

export class ProjectUpcoming extends Component {
    constructor(props) {
        super(props);

        this.publicFunctions = props.publicObjects.functions;
        this.enums = props.publicObjects.enums;
        this.constants = props.publicObjects.constants;

        this.state = {
            data: this.publicFunctions.getServerObject(this.constants.OBJECT_PROJECT_PIPELINE),
            turnName: Utility.turnNameFromNumber(this.publicFunctions.getServerObject(this.constants.OBJECT_GAME).turnNumber)
        };

        this.onOpenProjPipe = this.onOpenProjPipe.bind(this);
        this.updateUpcomingProjects = this.updateUpcomingProjects.bind(this);
        this.updateGameTurn = this.updateGameTurn.bind(this);
        this.renderProjPipeModal = this.renderProjPipeModal.bind(this);

        this.publicFunctions.addListener(this.constants.OBJECT_PROJECT_PIPELINE, this.updateUpcomingProjects);
        this.publicFunctions.addListener(this.constants.OBJECT_GAME, this.updateGameTurn);

        this.projectListListener = (a) => "";
        this.addUpcomingProjectsListener = this.addUpcomingProjectsListener.bind(this);
    }

    componentWillUnmount() {
        this.publicFunctions.removeListener(this.constants.OBJECT_PROJECT_PIPELINE, this.updateUpcomingProjects);
        this.publicFunctions.removeListener(this.constants.OBJECT_GAME, this.updateGameTurn);
    }

    updateUpcomingProjects(data) {
        this.setState({ data: data });
        this.projectListListener(data.biddableProjects);
    }

    addUpcomingProjectsListener(listener) {
        this.projectListListener = listener;
    }

    updateGameTurn(data) {
        this.setState({ turnName: Utility.turnNameFromNumber(data.turnNumber) });;
    }

    renderProjPipeModal() {
        return (
            <div>
                <ProjectPipeline publicObjects={this.publicFunctions.getPublicObjects()} />
            </div>
        );
    }

    onOpenProjPipe(e) {
        e.preventDefault();
        this.publicFunctions.launchOkModal(this.renderProjPipeModal, this.onCloseProjPipe);
    }

    render() {
        if (this.state.data === null) {
            return "";
        }
        return (
            <div>
                Current turn: {this.state.turnName}<br />
                <ProjectList title="Biddable projects this turn:"
                    projects={this.state.data.biddableProjects}
                    itemListener={this.addUpcomingProjectsListener}
                    inProgress={false}
                    showReqsMet={true}
                    />
            </div>
        );
    }
}

export class ProjectList extends Component {
    constructor(props) {
        super(props);

        if (props.showReqsMet) {
            this.headerRenderer = this.renderProjectHeaderWithReqs.bind(this);
        }
        else {
            this.headerRenderer = this.renderProjectHeader.bind(this);
        }

        if (props.actionRenderer) {
            this.contentsRenderer = this.renderProjectWithAction.bind(this);
        }
        else {
            this.contentsRenderer = this.renderProjectOnly.bind(this);
        }
    }

    renderProjectHeader(proj) {
        return (
            <div>
                <b>{proj.name}</b>
            </div>
        );
    }

    renderProjectHeaderWithReqs(proj) {
        let matchesReqs = proj.canBid;
        let qualificationMarker = matchesReqs ? "[O]" : "[X]";
        let style = {
            backgroundColor: matchesReqs ? "rgb(173, 255, 47, 0.4)" : "rgb(255, 69, 0, 0.4)"
        }
        return (
            <div style={style}>
                <b>{qualificationMarker}{proj.name}</b>
            </div>
        );
    }

    renderProjectOnly(proj) {
        return (
            <div>
                <ProjectStaticDisplay projData={proj} inProgress={this.props.inProgress} excludeName={true} />
            </div>
        );
    }

    renderProjectWithAction(proj, index) {
        return (
            <div>
                <ProjectStaticDisplay projData={proj} inProgress={this.props.inProgress} excludeName={true} /><br />
                {this.props.actionRenderer(proj, index)}
            </div>
        );
    }

    render() {
        return this.props.projects === undefined || this.props.projects.length === 0 ? "" :
            <ExpandableList
                title={this.props.title}
                items={this.props.projects}
                headerRenderer={this.headerRenderer}
                contentRenderer={this.contentsRenderer}
                itemListener={this.props.itemListener}
            />;
    }
}