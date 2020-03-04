function enablePhone() {
    let code = document.getElementById("countryCodeDrop");

    if (code.value) {
        document.getElementById("profilePhoneNumber").disabled = false;
    }
}