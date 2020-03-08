'use strict';

let inputRoleForUserInRole = document.querySelector('.addRoleValueForUserInRole');
let dropdownRolesForUserInRole = document.querySelector('.rolesListForUserInRole');
let dropdownRoleItemsForUserInRow = Array.from(document.querySelectorAll('#roleForUserInRole'));
inputRoleForUserInRole.focus(); // Demo purposes only

var newRolesForUserInRoleValueArray = [];
dropdownRoleItemsForUserInRow.forEach(function (item) {
    newRolesForUserInRoleValueArray.push(item.textContent);
});

var closeDropdown = function closeDropdown() {
    dropdownRolesForUserInRole.classList.remove('open');
};

inputRoleForUserInRole.addEventListener('input', function () {
    dropdownRolesForUserInRole.classList.add('open');
    var inputValue = inputRoleForUserInRole.value.toLowerCase();
    var valueSubstring = undefined;
    if (inputValue.length > 0) {
        for (var j = 0; j < newRolesForUserInRoleValueArray.length; j++) {
            if (!(inputValue.substring(0, inputValue.length) === newRolesForUserInRoleValueArray[j].substring(0, inputValue.length).toLowerCase())) {
                dropdownRoleItemsForUserInRow[j].classList.add('closed');
            } else {
                dropdownRoleItemsForUserInRow[j].classList.remove('closed');
            }
        }
    } else {
        for (var i = 0; i < dropdownRoleItemsForUserInRow.length; i++) {
            dropdownRoleItemsForUserInRow[i].classList.remove('closed');
        }
    }
});

dropdownRoleItemsForUserInRow.forEach(function (item) {
    item.addEventListener('click', function (evt) {
        inputRoleForUserInRole.value = item.textContent;
        dropdownRoleItemsForUserInRow.forEach(function (dropdown) {
            dropdown.classList.add('closed');
        });
    });
});

inputRoleForUserInRole.addEventListener('focus', function () {
    inputRoleForUserInRole.placeholder = 'Type to filter';
    dropdownRolesForUserInRole.classList.add('open');
    dropdownRoleItemsForUserInRow.forEach(function (dropdown) {
        dropdown.classList.remove('closed');
    });
});

inputRoleForUserInRole.addEventListener('blur', function () {
    inputRoleForUserInRole.placeholder = 'Select role';
    dropdownRolesForUserInRole.classList.remove('open');
});

document.addEventListener('click', function (evt) {
    var isDropdown = dropdownRolesForUserInRole.contains(evt.target);
    var isInput = inputRoleForUserInRole.contains(evt.target);
    if (!isDropdown && !isInput) {
        dropdownRolesForUserInRole.classList.remove('open');
    }
});