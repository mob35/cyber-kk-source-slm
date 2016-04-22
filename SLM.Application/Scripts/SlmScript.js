function ChkNotThaiCharacter(e) {
    var charCode = (e.which) ? e.which : event.keyCode
    if (charCode >= 3585 && charCode <= 3675)                         //UTF8 ก=3585 
        return false;
    else
        return true;
}
function ChkInt(e) {
    var charCode = (e.which) ? e.which : event.keyCode
    if (e.ctrlKey == true) {
        if (charCode == 99 || charCode == 118 || charCode == 120)   //99=c, 118=v, 120=x
            return true;
        else
            return false;
    }
    else {
        if (charCode != 8 && (charCode < 48 || charCode > 57))  //8=backspace, 48-57=0-9
            return false;
        else
            return true;
    }
}
function ChkIntOnBlur(textbox, labelId, errmsg) {
    if (isNaN(textbox.value)) {
        document.getElementById(labelId).innerHTML = errmsg;
    }
}
function ChkIntOnBlurClear(textbox) {
    if (isNaN(textbox.value)) {
        textbox.value = '';
        textbox.focus();
    }
}

function ChkDbl(e, ctl) {
    var charCode = (e.which) ? e.which : event.keyCode
    if (charCode != 8 && (charCode < 48 || charCode > 57)) {
        if (charCode == 46) {
            if (ctl.value.indexOf(".", 0) >= 0)
                return false;
            else
                return true;
        }
        else
            return false;
    }
    else {
        return true;
    }
}
function valDbl(ctlz) {
    ctlz.value = formatDbl(ctlz.value);
    ctlz.value = ClearMinus(ctlz.value);
    ctlz.value = AddComma(ctlz.value, ctlz.value.length - 3);
}
function ClearMinus(valIn) {
    temp = valIn;
    while (temp.indexOf("-", 0) != -1)
        temp = temp.replace("-", "");

    return temp;
}
function AddComma(valIn, posStart) {
    if (parseFloat(valIn) < 0.00)
        j = 4;
    else
        j = 3;

    var i = posStart;
    var temp = valIn;
    while (i > j) {
        i = i - 3;
        temp = temp.substring(0, i) + "," + temp.substring(i, temp.length);
    }
    return temp;
}
function prepareNum(ctlz) {
    ctlz.value = ClearComma(ctlz.value);
    ctlz.select();
}
function ClearComma(valIn) {
    temp = valIn;
    while (temp.indexOf(",", 0) != -1)
        temp = temp.replace(",", "");

    return temp;

}
function formatDbl(valIn) {
    var temp = valIn;
    if (temp.replace(" ", "") == '')
        return ''
    else {
        if (isNaN(parseFloat(temp))) {
            temp = 0;
        }
        var temp = "" + Math.round(parseFloat(temp) * 100);
        if (temp == 0)
            return '';
        else {
            if (parseFloat(temp) < 0) {
                temp = temp.substring(1, temp.length);
                var i = temp.length;
                while (i < 3) {
                    temp = "0" + temp;
                    i = i + 1;
                }
                i = i - 2;
                temp = "-" + temp.substring(0, i) + "." + temp.substring(i, temp.length);

            }
            else {
                var i = temp.length;
                while (i < 3) {
                    temp = "0" + temp;
                    i = i + 1;
                }
                i = i - 2;
                temp = temp.substring(0, i) + "." + temp.substring(i, temp.length);
            }
            return temp;
        }
    }
}
//Check Max Length for multiline textbox
function validateLimit(obj, divID, maxchar) {

    objDiv = document.getElementById(divID);

    if (this.id) obj = this;

    var remaningChar = maxchar - trimEnter(obj.value.trim()).length;

    if (remaningChar <= 0) {
        obj.value = obj.value.substring(maxchar, 0);
        if (objDiv.id) {
            objDiv.innerHTML = "Input reaches limit of " + maxchar + " characters";
        }
        return false;
    }
    else {
        objDiv.innerHTML = '';
        return true;
    }
}

function trimEnter(dataStr) {
    return dataStr.replace(/(\r\n|\r|\n)/g, "");
}
// --------------------------------------------

function ChkMultilineMaxLength(e, ctl, maxlength) {
    if (ctl.value.trim().length >= maxlength) {
        var charCode = (e.which) ? e.which : event.keyCode
        if (charCode != 8)
            return false;
        else
            return true;
    }
    else {
        return true;
    }
}

//Check Max Length for decimal textbox
//if pass, show number with format in textbox
//if fail, show error message in label
function valDbl2(textbox, labelId, errmsg, maxlength) {
    var val = textbox.value;

    if (val.indexOf(".") >= 0) {
        var vals = val.split('.');
        if (vals[0].trim().length == 0 && vals[1].trim().length == 0)
            textbox.value = '0.00';
        else if (vals[0].trim().length > maxlength)
            document.getElementById(labelId).innerHTML = errmsg;
        else {
            document.getElementById(labelId).innerHTML = '';
            textbox.value = formatDbl(textbox.value);
            textbox.value = ClearMinus(textbox.value);
            textbox.value = AddComma(textbox.value, textbox.value.length - 3);
        }
    }
    else {
        if (val.trim() == '') {
            textbox.value = '';
            document.getElementById(labelId).innerHTML = '';
        }
        else {
            if (val.length > maxlength)
                document.getElementById(labelId).innerHTML = errmsg;
            else {
                document.getElementById(labelId).innerHTML = '';
                textbox.value = formatDbl(textbox.value);
                textbox.value = ClearMinus(textbox.value);
                textbox.value = AddComma(textbox.value, textbox.value.length - 3);
            }
        }
    }
}

//Percent value for decimal textbox
//if pass, show number with format in textbox
//if fail, show error message in label
function valPercent(textbox, labelId, errmsg) {
    var val = textbox.value;

    if (val.indexOf(".") >= 0) {
        var vals = val.split('.');
        if (vals[0].trim().length == 0 && vals[1].trim().length == 0)
            textbox.value = '0.00';
        else if (parseFloat(val) > 100)
            document.getElementById(labelId).innerHTML = errmsg;
        else {
            document.getElementById(labelId).innerHTML = '';
            textbox.value = formatDbl(textbox.value);
            textbox.value = ClearMinus(textbox.value);
            textbox.value = AddComma(textbox.value, textbox.value.length - 3);
        }
    }
    else {
        if (val.trim() == '') {
            textbox.value = '';
            document.getElementById(labelId).innerHTML = '';
        }
        else {
            if (parseFloat(val) > 100)
                document.getElementById(labelId).innerHTML = errmsg;
            else {
                document.getElementById(labelId).innerHTML = '';
                textbox.value = formatDbl(textbox.value);
                textbox.value = ClearMinus(textbox.value);
                textbox.value = AddComma(textbox.value, textbox.value.length - 3);
            }
        }
    }
}