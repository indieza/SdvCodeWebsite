'use strict';

var inputRoleField = document.querySelector('#usernameRoleValue');
var dropdownRole = document.querySelector('#usernamesRolesList');
var dropdownRoleArray = [].concat(document.querySelectorAll('#usernameRoleItem'));
var dropdownRoleItems = dropdownRoleArray[0];
//dropdown.classList.add('open');
inputRoleField.focus(); // Demo purposes only

var valueRoleArray = [];
dropdownRoleItems.forEach(function (item) {
    valueRoleArray.push(item.textContent);
});

var closeDropdown = function closeDropdown() {
    dropdownRole.classList.remove('open');
};

inputRoleField.addEventListener('input', function () {
    dropdownRole.classList.add('open');
    var inputValue = inputRoleField.value.toLowerCase();
    var valueSubstring = undefined;
    if (inputValue.length > 0) {
        for (var j = 0; j < valueRoleArray.length; j++) {
            if (!(inputValue.substring(0, inputValue.length) === valueRoleArray[j].substring(0, inputValue.length).toLowerCase())) {
                dropdownRoleItems[j].classList.add('closed');
            } else {
                dropdownRoleItems[j].classList.remove('closed');
            }
        }
    } else {
        for (var i = 0; i < dropdownRoleItems.length; i++) {
            dropdownRoleItems[i].classList.remove('closed');
        }
    }
});

dropdownRoleItems.forEach(function (item) {
    item.addEventListener('click', function (evt) {
        inputRoleField.value = item.textContent;
        dropdownRoleItems.forEach(function (dropdown) {
            dropdown.classList.add('closed');
        });
    });
});

inputRoleField.addEventListener('focus', function () {
    inputRoleField.placeholder = 'Type role to filter';
    dropdownRole.classList.add('open');
    dropdownRoleItems.forEach(function (dropdown) {
        dropdown.classList.remove('closed');
    });
});

inputRoleField.addEventListener('blur', function () {
    inputRoleField.placeholder = 'Select role';
    dropdownRole.classList.remove('open');
});

document.addEventListener('click', function (evt) {
    var isDropdown = dropdownRole.contains(evt.target);
    var isInput = inputRoleField.contains(evt.target);
    if (!isDropdown && !isInput) {
        dropdownRole.classList.remove('open');
    }
});