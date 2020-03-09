'use strict';

let inputUserForUserRemoveRole = document.querySelector('.addUserValueForUserRemoveRole');
let dropdownUsersForUserRemoveRole = document.querySelector('.usersListForUserRemoveRole');
let dropdownUserItemsForUserRemoveRole = Array.from(document.querySelectorAll('#userForUserRemoveRole'));
inputUserForUserRemoveRole.focus(); // Demo purposes only

var newUsersForUserRemoveRoleValueArray = [];
dropdownUserItemsForUserRemoveRole.forEach(function (item) {
    newUsersForUserRemoveRoleValueArray.push(item.textContent);
});

var closeDropdown = function closeDropdown() {
    dropdownUsersForUserRemoveRole.classList.remove('open');
};

inputUserForUserRemoveRole.addEventListener('input', function () {
    dropdownUsersForUserRemoveRole.classList.add('open');
    var inputValue = inputUserForUserRemoveRole.value.toLowerCase();
    var valueSubstring = undefined;
    if (inputValue.length > 0) {
        for (var j = 0; j < newUsersForUserRemoveRoleValueArray.length; j++) {
            if (!(inputValue.substring(0, inputValue.length) === newUsersForUserRemoveRoleValueArray[j].substring(0, inputValue.length).toLowerCase())) {
                dropdownUserItemsForUserRemoveRole[j].classList.add('closed');
            } else {
                dropdownUserItemsForUserRemoveRole[j].classList.remove('closed');
            }
        }
    } else {
        for (var i = 0; i < dropdownUserItemsForUserRemoveRole.length; i++) {
            dropdownUserItemsForUserRemoveRole[i].classList.remove('closed');
        }
    }
});

dropdownUserItemsForUserRemoveRole.forEach(function (item) {
    item.addEventListener('click', function (evt) {
        inputUserForUserRemoveRole.value = item.textContent;
        dropdownUserItemsForUserRemoveRole.forEach(function (dropdown) {
            dropdown.classList.add('closed');
        });
    });
});

inputUserForUserRemoveRole.addEventListener('focus', function () {
    inputUserForUserRemoveRole.placeholder = 'Type to filter';
    dropdownUsersForUserRemoveRole.classList.add('open');
    dropdownUserItemsForUserRemoveRole.forEach(function (dropdown) {
        dropdown.classList.remove('closed');
    });
});

inputUserForUserRemoveRole.addEventListener('blur', function () {
    inputUserForUserRemoveRole.placeholder = 'Select user';
    dropdownUsersForUserRemoveRole.classList.remove('open');
});

document.addEventListener('click', function (evt) {
    var isDropdown = dropdownUsersForUserRemoveRole.contains(evt.target);
    var isInput = inputUserForUserRemoveRole.contains(evt.target);
    if (!isDropdown && !isInput) {
        dropdownUsersForUserRemoveRole.classList.remove('open');
    }
});