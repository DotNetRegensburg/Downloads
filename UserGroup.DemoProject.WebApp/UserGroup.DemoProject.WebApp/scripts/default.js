//for the Messaging Module
function SelectAll(CheckBoxControl) {

    if (CheckBoxControl.checked == true) {
        var i;
        for (i = 0; i < document.forms[0].elements.length; i++) {
            if (document.forms[0].elements[i].type == 'checkbox') {
                document.forms[0].elements[i].checked = true;
            }
        }
    }
    else {
        var i;
        for (i = 0; i < document.forms[0].elements.length; i++) {
            if (document.forms[0].elements[i].type == 'checkbox') {
                document.forms[0].elements[i].checked = false;
            }
        }
    }

}

function GetCheckBox() {

    var str = '';
    var sep = ';';
    // return document.getElementById(CheckBoxControl).checked;

    for (i = 0; i < document.forms[0].elements.length; i++) {
        if (document.forms[0].elements[i].type == 'checkbox') {
            if (document.forms[0].elements[i].id != 'chkMaster') {
                if (document.forms[0].elements[i].checked == true) {
                    str = sep + (document.forms[0].elements[i].id) + str;
                }
            }
        }
    }

    return str;
}