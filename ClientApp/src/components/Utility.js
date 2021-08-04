class Utility {
    turnNameFromNumber(number) {
        return `${2020 + Math.floor(number / 4)} Q${(number % 4) + 1}`
    }

    moneyString(money) {
        if (money === 0) {
            return "$0";
        }
        return `$${money.toLocaleString()},000`;
    }

    modifierToPercentage(modifier) {
        let dotIndex = modifier.indexOf(".");
        if (dotIndex === -1) {
            return modifier + "00%";
        }
        let splitArr = modifier.split(".");
        let afterDot = splitArr[1];
        if (afterDot.length > 2) {
            afterDot = afterDot.substring(0, 2);
        }
        if (afterDot.length === 1) {
            afterDot = afterDot + "0";
        }
        if (afterDot.length === 0) {
            afterDot = "00"
        }
        let beforeDot = splitArr[0];
        if (beforeDot - 0 === 0) {
            beforeDot = "";
        }
        return beforeDot + afterDot + "%"
    }
}

export default new Utility()