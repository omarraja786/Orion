// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function decodeHtml(html) {
    var txt = document.createElement("textarea");
    txt.innerHTML = html;
    return txt.value;
}

function basicJS(data) {
    //var generatedHtml = data.replace(/&/g, "&amp;").replace(/>/g, "&gt;").replace(/</g, "&lt;").replace(/"/g, "&quot;");
    var text = decodeHtml(data);
    window.external.OutlookWinApp.InsertHtml(text);
}

function Validate() {
    document.getElementById("btnSubmit").disabled = true;
    var elem = document.getElementById("btnSubmit");
    var name = document.getElementById("name");
    var position = document.getElementById("position");
    var area = document.getElementById("area");
    var company = document.getElementById("company");
    var industry = document.getElementById("industry");
    var size = document.getElementById("size");
    var valuePropInput = document.getElementById("valuePropInput");
    var product = document.getElementById("product");
    var myName = document.getElementById("myName");
    var myTitle = document.getElementById("myTitle");
    var myCompany = document.getElementById("myCompany");

    var strName = name.value;
    var strPosition = position.options[position.selectedIndex].value;
    var strArea = area.options[area.selectedIndex].value;
    var strCompany = company.value;
    var strIndustry = industry.value;
    var strSize = size.options[size.selectedIndex].value;
    var strValuePropInput = valuePropInput.value;
    var strProduct = product.value;
    var strMyName = myName.value;
    var strMyTitle = myTitle.value;
    var strMyCompany = myCompany.value;

    if (strName === "" ||
        strPosition === "" ||
        strArea === "" ||
        strCompany === "" ||
        strIndustry === "" ||
        strSize === "" ||
        strValuePropInput === "" ||
        strProduct === "" ||
        strMyName === "" ||
        strMyTitle === "" ||
        strMyCompany === "") {
        alert("Please correctly fill in all the fields");
        document.getElementById("btnSubmit").disabled = false;
        return false;
    } else {
        elem.value = "Generating Email...";
    }

}



var currentHtml = null;
function processClick(data) {
    // for simplicity, generate HTML from data)
    Office.onReady(function() {

        //var sData = JSON.stringify(data);
        var generatedHtml = sData.replace(/&/g, "&amp;").replace(/>/g, "&gt;").replace(/</g, "&lt;")
            .replace(/"/g, "&quot;");
        if (window.external.OutlookWinApp != null) {
            window.external.OutlookWinApp.InsertHtml(generatedHtml);
        } else {
            if (currentHtml == null) {
                console.log(currentHtml)
                Office.context.mailbox.item.body.getAsync(Office.CoercionType.Html,
                    function(asyncResult) {
                        if (asyncResult.status != "failed") {
                            currentHtml = asyncResult.value;
                        }
                        processSubmitData(generatedHtml);
                    });
            } else {
                processSubmitData(generatedHtml);
            }
        }
    });
}

function processSubmitData(generatedHtml) {
    var setHtml = generatedHtml + currentHtml;
    Office.context.mailbox.item.body.setAsync(setHtml, { coercionType: Office.CoercionType.Html });

}
