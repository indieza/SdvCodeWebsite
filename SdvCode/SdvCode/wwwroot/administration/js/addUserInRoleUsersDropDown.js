'use strict';

let inputUserForUserInRole = document.querySelector('.addUserValueForUserInRole');
let dropdownUsersForUserInRole = document.querySelector('.usersListForUserInRole');
let dropdownUserItemsForUserInRole = Array.from(document.querySelectorAll('#userForUserInRole'));
inputUserForUserInRole.focus(); // Demo purposes only

var newUsersForUserInRoleValueArray = [];
dropdownUserItemsForUserInRole.forEach(function (item) {
    newUsersForUserInRoleValueArray.push(item.textContent);
});

var closeDropdown = function closeDropdown() {
    dropdownUsersForUserInRole.classList.remove('open');
};

inputUserForUserInRole.addEventListener('input', function () {
    dropdownUsersForUserInRole.classList.add('open');
    var inputValue = inputUserForUserInRole.value.toLowerCase();
    var valueSubstring = undefined;
    if (inputValue.length > 0) {
        for (var j = 0; j < newUsersForUserInRoleValueArray.length; j++) {
            if (!(inputValue.substring(0, inputValue.length) === newUsersForUserInRoleValueArray[j].substring(0, inputValue.length).toLowerCase())) {
                dropdownUserItemsForUserInRole[j].classList.add('closed');
            } else {
                dropdownUserItemsForUserInRole[j].classList.remove('closed');
            }
        }
    } else {
        for (var i = 0; i < dropdownUserItemsForUserInRole.length; i++) {
            dropdownUserItemsForUserInRole[i].classList.remove('closed');
        }
    }
});

dropdownUserItemsForUserInRole.forEach(function (item) {
    item.addEventListener('click', function (evt) {
        inputUserForUserInRole.value = item.textContent;
        dropdownUserItemsForUserInRole.forEach(function (dropdown) {
            dropdown.classList.add('closed');
        });
    });
});

inputUserForUserInRole.addEventListener('focus', function () {
    inputUserForUserInRole.placeholder = 'Type to filter';
    dropdownUsersForUserInRole.classList.add('open');
    dropdownUserItemsForUserInRole.forEach(function (dropdown) {
        dropdown.classList.remove('closed');
    });
});

inputUserForUserInRole.addEventListener('blur', function () {
    inputUserForUserInRole.placeholder = 'Select user';
    dropdownUsersForUserInRole.classList.remove('open');
});

document.addEventListener('click', function (evt) {
    var isDropdown = dropdownUsersForUserInRole.contains(evt.target);
    var isInput = inputUserForUserInRole.contains(evt.target);
    if (!isDropdown && !isInput) {
        dropdownUsersForUserInRole.classList.remove('open');
    }
});