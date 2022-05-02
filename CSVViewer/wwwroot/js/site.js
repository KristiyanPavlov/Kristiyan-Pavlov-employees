// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function showCultureDropDown() {
    document.getElementById("cultureinfo-dropdown").classList.toggle("show");
    document.getElementById("searchCulture").focus();
}

function filterCultures() {
    let input, filter, a, i;
    input = document.getElementById("searchCulture");
    filter = input.value.toUpperCase();
    div = document.getElementById("cultureinfo-dropdown");
    a = div.getElementsByTagName("a");
    for (i = 0; i < a.length; i++) {
        txtValue = a[i].textContent || a[i].innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
            a[i].style.display = "";
        } else {
            a[i].style.display = "none";
        }
    }
}

function SetValue(value, displayValue) {
    document.getElementById("cultureInfos").value = value;
    document.getElementById("cultureInfosValue").value = displayValue;
}