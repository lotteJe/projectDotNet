/* Autocomplete Google Places */

var placeSearch, autocomplete;
var componentForm = {
    route: 'long_name',
    street_number: 'short_name',
    postal_code: 'short_name',
    locality: 'long_name'
    // administrative_area_level_1: 'long_name',
    //   country: 'long_name',

};

function initAutocomplete() {
    // Create the autocomplete object, restricting the search to geographical
    // location types.
    autocomplete = new google.maps.places.Autocomplete(
        /** @type {!HTMLInputElement} */(document.getElementById('autocomplete')),
        { types: ['geocode'] });

    /*enkel belgië en nederland */
    autocomplete.setComponentRestrictions(
            { 'country': ['be', 'nl'] });

    // When the user selects an address from the dropdown, populate the address
    // fields in the form.
    autocomplete.addListener('place_changed', fillInAddress);
}

function fillInAddress() {
    // Get the place details from the autocomplete object.
    var place = autocomplete.getPlace();

    var e = document.getElementById("autocomplete");
    e.id = "route";
    for (var component in componentForm) {
        document.getElementById(component).value = '';
        document.getElementById(component).disabled = false;
    }

    // Get each component of the address from the place details
    // and fill the corresponding field on the form.
    for (var i = 0; i < place.address_components.length; i++) {
        var addressType = place.address_components[i].types[0];
        if (componentForm[addressType]) {
            var val = place.address_components[i][componentForm[addressType]];
            document.getElementById(addressType).value = val;
        }
    }
    var e = document.getElementById("route");
    e.id = "autocomplete";
}
$("#autocomplete").keydown(function (event) {
    if (event.keyCode === 13) {
        event.preventDefault();
        return false;
    }
});

/* active link nav*/
$(function () {
    $("aside[data-navigation='true']").find("li").children("a").each(function () {
        if ($(this).attr("href") === window.location.pathname) {
            $(this).parent().addClass("active");
        }
    });
});

$(function () {
    $("aside[data-navigation='true']").find("li").children("a").each(function () {
        if ($(this).attr("href") === window.location.pathname) {
            $(this).parent().addClass("active");
        }
    });
});

/* Laad partial view bij nieuwe analyse */
$('#show-partial').click(function () {
    $('#partial').show();
});
$('#show-baten').click(function () {
    $('#batenpartial').show();
    $('#overzichtpartial').hide();
    $('#kostenpartial').hide();
    $('#show-overzicht').removeClass("active");
    $('#show-baten').addClass("active");
    $('#show-kosten').removeClass("active");
});
$('#show-overzicht').click(function () {
    $('#batenpartial').hide();
    $('#overzichtpartial').show();
    $('#kostenpartial').hide();
    $('#show-overzicht').addClass("active");
    $('#show-baten').removeClass("active");
    $('#show-kosten').removeClass("active");
});
$('#show-kosten').click(function () {
    $('#batenpartial').hide();
    $('#overzichtpartial').hide();
    $('#kostenpartial').show();
    $('#show-overzicht').removeClass("active");
    $('#show-baten').removeClass("active");
    $('#show-kosten').addClass("active");
});


/*kostenpartial */

//$('#lijnEdit').click(function () {
//    var z = document.getElementById("demo");
//    z.classList.add("in");

//});
//$('#lijnEdit').click(function () {
//    $('#partialK').show();
//});
//$("#toevoegenFunctie").click(function () {
//    $("#partialK").slideToggle();
//});
//$(".rotate").click(function () {
//    $(this).toggleClass("down");
//    $('#partialK').hide();
//    var elements = document.getElementsByTagName("input");
//    for (var ii = 0; ii < elements.length; ii++) {
//        if (elements[ii].type === "text") {
//            elements[ii].value = "";
//        }
//        if (elements[ii].type === "number") {
//            elements[ii].value = 0;
//        }
//    }

//});

/* TABEL EXPORTEREN */

function fnExcelReport() {
    var tab_text = '<html xmlns:x="urn:schemas-microsoft-com:office:excel">';
    tab_text = tab_text + '<head><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet>';

    tab_text = tab_text + '<x:Name>Test Sheet</x:Name>';

    tab_text = tab_text + '<x:WorksheetOptions><x:Panes></x:Panes></x:WorksheetOptions></x:ExcelWorksheet>';
    tab_text = tab_text + '</x:ExcelWorksheets></x:ExcelWorkbook></xml></head><body>';

    tab_text = tab_text + "<table border='1px'>";
    tab_text = tab_text + $('#overzicht').html();
    tab_text = tab_text + '</table></body></html>';

    var data_type = 'data:application/vnd.ms-excel';
    var naam = $("#naamOrganisatie").text();
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");
    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
        if (window.navigator.msSaveBlob) {
            var blob = new Blob([tab_text], {
                type: "application/csv;charset=utf-8;"
            });
            navigator.msSaveBlob(blob, 'test.xls');
        }
    } else {
        $('#tabel').attr('href', data_type + ', ' + encodeURIComponent(tab_text));
        $('#tabel').attr('download', 'Analyse_' + naam + '.xls');
    }

}

/* Tooltip */
$(document).ready(function () {
    $('[tool-tip-toggle="tooltip"]').tooltip();
});

/* navigation */
$(document).ready(function () {
    function disableBack() { window.history.forward() }

    window.onload = disableBack();
    window.onpageshow = function (evt) { if (evt.persisted) disableBack() }
});

/* pdf*/
function makePdf() {
    var werkgever = document.getElementById("naamOrganisatie").innerText;
    var doc = new jsPDF('l');
    doc.setFontSize(18);
    doc.text(werkgever, 14, 22);
    doc.setFontSize(11);
    doc.setTextColor(100);
    var text = "Hieronder vindt u het overzicht van de kosten en baten voor deze organsiatie.";
    doc.text(text, 14, 30);
    var elem = document.getElementById("overzicht");
    var res = doc.autoTableHtmlToJson(elem);
    doc.autoTable(res.columns, res.data, { startY: 40, styles: { overflow: 'linebreak' } });
    doc.save('Analyse_'+werkgever +'.pdf');

}



/* confirm */

$(document).on("click", "#myLink", function (e) {
    var link = $(this).attr("href"); 
    e.preventDefault();
    bootbox.setLocale("nl");
    bootbox.confirm("U staat op het punt deze analyse te verwijderen. <br/> Bent u zeker dat u wil doorgaan?", function (result) {
        if (result) {
            document.location.href = link;     
        }
    });
});

