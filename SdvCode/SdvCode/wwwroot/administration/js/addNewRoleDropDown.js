'use strict';

let inputNewRole = document.querySelector('.addNewRoleValue');
let dropdownNewRoles = document.querySelector('.newRolesList');
let dropdownNewRowItems = Array.from(document.querySelectorAll('#addRole'));

var newRolesForUserInRoleValueArray = [];
dropdownNewRowItems.forEach(function (item) {
    newRolesForUserInRoleValueArray.push(item.textContent);
});

var closeDropdown = function closeDropdown() {
    dropdownNewRoles.classList.remove('open');
};

inputNewRole.addEventListener('input', function () {
    dropdownNewRoles.classList.add('open');
    var inputValue = inputNewRole.value.toLowerCase();
    var valueSubstring = undefined;
    if (inputValue.length > 0) {
        for (var j = 0; j < newRolesForUserInRoleValueArray.length; j++) {
            if (!(inputValue.substring(0, inputValue.length) === newRolesForUserInRoleValueArray[j].substring(0, inputValue.length).toLowerCase())) {
                dropdownNewRowItems[j].classList.add('closed');
            } else {
                dropdownNewRowItems[j].classList.remove('closed');
            }
        }
    } else {
        for (var i = 0; i < dropdownNewRowItems.length; i++) {
            dropdownNewRowItems[i].classList.remove('closed');
        }
    }
});

dropdownNewRowItems.forEach(function (item) {
    item.addEventListener('click', function (evt) {
        inputNewRole.value = item.textContent;
        dropdownNewRowItems.forEach(function (dropdown) {
            dropdown.classList.add('closed');
        });
    });
});

inputNewRole.addEventListener('focus', function () {
    inputNewRole.placeholder = 'Type to filter';
    dropdownNewRoles.classList.add('open');
    dropdownNewRowItems.forEach(function (dropdown) {
        dropdown.classList.remove('closed');
    });
});

inputNewRole.addEventListener('blur', function () {
    inputNewRole.placeholder = 'Select role';
    dropdownNewRoles.classList.remove('open');
});

document.addEventListener('click', function (evt) {
    var isDropdown = dropdownNewRoles.contains(evt.target);
    var isInput = inputNewRole.contains(evt.target);
    if (!isDropdown && !isInput) {
        dropdownNewRoles.classList.remove('open');
    }
});