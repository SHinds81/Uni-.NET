// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $("#typeSelection").click(function () {
        var value = document.getElementById('typeSelection').value;
        
        if (value == 'Transfer') {
            document.getElementById('destinationNum').hidden = false;
            document.getElementById('destinationSelect').hidden = true;
        }
        {
            document.getElementById('destinationNum').hidden = true;
            document.getElementById('destinationSelect').hidden = true;
        }

    })
})

$(function () {
    $("#loginSelection").click(function () {
        var value = document.getElementById('loginSelection').value;
        console.log(value);
        if (value == 'TEMP_LOCKED') {
            document.getElementById('lockedUntil').hidden = false;
        }
        else {
            document.getElementById('lockedUntil').hidden = true;
        }


    })
})

$('#accountDropDown').change(function () {

    /* Get the selected value of dropdownlist */
    document.getElementById('buttonSubmit').click();

});