'use strict';

let inputRoleForUserRemoveRole = document.querySelector('.removeRoleValueForUserInRole');
let dropdownRolesForUserRemoveRole = document.querySelector('.rolesListForUserRemoveRole');
let dropdownRoleItemsForUserRemoveRole = Array.from(document.querySelectorAll('#roleForUserRemoveRole'));

var newRolesForUserRemoveRoleValueArray = [];
dropdownRoleItemsForUserRemoveRole.forEach(function (item) {
    newRolesForUserRemoveRoleValueArray.push(item.textContent);
});

var closeDropdown = function closeDropdown() {
    dropdownRolesForUserRemoveRole.classList.remove('open');
};

inputRoleForUserRemoveRole.addEventListener('input', function () {
    dropdownRolesForUserRemoveRole.classList.add('open');
    var inputValue = inputRoleForUserRemoveRole.value.toLowerCase();
    var valueSubstring = undefined;
    if (inputValue.length > 0) {
        for (var j = 0; j < newRolesForUserRemoveRoleValueArray.length; j++) {
            if (!(inputValue.substring(0, inputValue.length) === newRolesForUserRemoveRoleValueArray[j].substring(0, inputValue.length).toLowerCase())) {
                dropdownRoleItemsForUserRemoveRole[j].classList.add('closed');
            } else {
                dropdownRoleItemsForUserRemoveRole[j].classList.remove('closed');
            }
        }
    } else {
        for (var i = 0; i < dropdownRoleItemsForUserRemoveRole.length; i++) {
            dropdownRoleItemsForUserRemoveRole[i].classList.remove('closed');
        }
    }
});

dropdownRoleItemsForUserRemoveRole.forEach(function (item) {
    item.addEventListener('click', function (evt) {
        inputRoleForUserRemoveRole.value = item.textContent;
        dropdownRoleItemsForUserRemoveRole.forEach(function (dropdown) {
            dropdown.classList.add('closed');
        });
    });
});

inputRoleForUserRemoveRole.addEventListener('focus', function () {
    inputRoleForUserRemoveRole.placeholder = 'Type to filter';
    dropdownRolesForUserRemoveRole.classList.add('open');
    dropdownRoleItemsForUserRemoveRole.forEach(function (dropdown) {
        dropdown.classList.remove('closed');
    });
});

inputRoleForUserRemoveRole.addEventListener('blur', function () {
    inputRoleForUserRemoveRole.placeholder = 'Select role';
    dropdownRolesForUserRemoveRole.classList.remove('open');
});

document.addEventListener('click', function (evt) {
    var isDropdown = dropdownRolesForUserRemoveRole.contains(evt.target);
    var isInput = inputRoleForUserRemoveRole.contains(evt.target);
    if (!isDropdown && !isInput) {
        dropdownRolesForUserRemoveRole.classList.remove('open');
    }
});