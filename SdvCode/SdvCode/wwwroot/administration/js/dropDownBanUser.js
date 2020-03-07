'use strict';

var inputBanUserField = document.querySelector('#baneUserValue');
var dropdownBanUsers = document.querySelector('#usersForBaneList');
var dropdownBanUsersArray = [].concat(document.querySelectorAll('#banItem'));
var dropdownBanUserItems = dropdownBanUsersArray[0];
//dropdownBanUsers.classList.add('open');
inputBanUserField.focus(); // Demo purposes only

var banUsersValueArray = [];
dropdownBanUserItems.forEach(function (item) {
    banUsersValueArray.push(item.textContent);
});

var closeDropdown = function closeDropdown() {
    dropdownBanUsers.classList.remove('open');
};

inputBanUserField.addEventListener('input', function () {
    dropdownBanUsers.classList.add('open');
    var inputValue = inputBanUserField.value.toLowerCase();
    var valueSubstring = undefined;
    if (inputValue.length > 0) {
        for (var j = 0; j < banUsersValueArray.length; j++) {
            if (!(inputValue.substring(0, inputValue.length) === banUsersValueArray[j].substring(0, inputValue.length).toLowerCase())) {
                dropdownBanUserItems[j].classList.add('closed');
            } else {
                dropdownBanUserItems[j].classList.remove('closed');
            }
        }
    } else {
        for (var i = 0; i < dropdownBanUserItems.length; i++) {
            dropdownBanUserItems[i].classList.remove('closed');
        }
    }
});

dropdownBanUserItems.forEach(function (item) {
    item.addEventListener('click', function (evt) {
        inputBanUserField.value = item.textContent;
        dropdownBanUserItems.forEach(function (dropdownBanUsers) {
            dropdownBanUsers.classList.add('closed');
        });
    });
});

inputBanUserField.addEventListener('focus', function () {
    inputBanUserField.placeholder = 'Type role to filter';
    dropdownBanUsers.classList.add('open');
    dropdownBanUserItems.forEach(function (dropdownBanUsers) {
        dropdownBanUsers.classList.remove('closed');
    });
});

inputBanUserField.addEventListener('blur', function () {
    inputBanUserField.placeholder = 'Select role';
    dropdownBanUsers.classList.remove('open');
});

document.addEventListener('click', function (evt) {
    var isDropdown = dropdownBanUsers.contains(evt.target);
    var isInput = inputBanUserField.contains(evt.target);
    if (!isDropdown && !isInput) {
        dropdownBanUsers.classList.remove('open');
    }
});