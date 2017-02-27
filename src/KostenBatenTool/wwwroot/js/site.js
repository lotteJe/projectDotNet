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

/* active link nav*/
$(function () {
    $("aside[data-navigation='true']").find("li").children("a").each(function () {
        if ($(this).attr("href") === window.location.pathname) {
            $(this).parent().addClass("active");
        }
    });
});
