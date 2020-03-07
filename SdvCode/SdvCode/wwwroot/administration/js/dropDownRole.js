'use strict';

var inputNewRoleField = document.querySelector('#roleValue');
var dropdownNewRoles = document.querySelector('#rolesList');
var dropdownNewRolesArray = [].concat(document.querySelectorAll('#roleItem'));
var dropdownNewRoleItems = dropdownNewRolesArray[0];
//dropdownNewRoles.classList.add('open');
inputNewRoleField.focus(); // Demo purposes only

var newRoleValueArray = [];
dropdownNewRoleItems.forEach(function (item) {
    newRoleValueArray.push(item.textContent);
});

var closeDropdown = function closeDropdown() {
    dropdownNewRoles.classList.remove('open');
};

inputNewRoleField.addEventListener('input', function () {
    dropdownNewRoles.classList.add('open');
    var inputValue = inputNewRoleField.value.toLowerCase();
    var valueSubstring = undefined;
    if (inputValue.length > 0) {
        for (var j = 0; j < newRoleValueArray.length; j++) {
            if (!(inputValue.substring(0, inputValue.length) === newRoleValueArray[j].substring(0, inputValue.length).toLowerCase())) {
                dropdownNewRoleItems[j].classList.add('closed');
            } else {
                dropdownNewRoleItems[j].classList.remove('closed');
            }
        }
    } else {
        for (var i = 0; i < dropdownNewRoleItems.length; i++) {
            dropdownNewRoleItems[i].classList.remove('closed');
        }
    }
});

dropdownNewRoleItems.forEach(function (item) {
    item.addEventListener('click', function (evt) {
        inputNewRoleField.value = item.textContent;
        dropdownNewRoleItems.forEach(function (dropdownNewRoles) {
            dropdownNewRoles.classList.add('closed');
        });
    });
});

inputNewRoleField.addEventListener('focus', function () {
    inputNewRoleField.placeholder = 'Type role to filter';
    dropdownNewRoles.classList.add('open');
    dropdownNewRoleItems.forEach(function (dropdownNewRoles) {
        dropdownNewRoles.classList.remove('closed');
    });
});

inputNewRoleField.addEventListener('blur', function () {
    inputNewRoleField.placeholder = 'Select role';
    dropdownNewRoles.classList.remove('open');
});

document.addEventListener('click', function (evt) {
    var isDropdown = dropdownNewRoles.contains(evt.target);
    var isInput = inputNewRoleField.contains(evt.target);
    if (!isDropdown && !isInput) {
        dropdownNewRoles.classList.remove('open');
    }
});