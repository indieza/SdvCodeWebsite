'use strict';

var inputUsernameField = document.querySelector('#usernameValue');
var dropdownUsernames = document.querySelector('#usernamesList');
var dropdownUsernamesArray = [].concat(document.querySelectorAll('#usernameItem'));
var dropdownUsernameItems = dropdownUsernamesArray[0];
//dropdownUsernames.classList.add('open');
inputUsernameField.focus(); // Demo purposes only

var usernameValueArray = [];
dropdownUsernameItems.forEach(function (item) {
    usernameValueArray.push(item.textContent);
});

var closeDropdown = function closeDropdown() {
    dropdownUsernames.classList.remove('open');
};

inputUsernameField.addEventListener('input', function () {
    dropdownUsernames.classList.add('open');
    var inputValue = inputUsernameField.value.toLowerCase();
    var valueSubstring = undefined;
    if (inputValue.length > 0) {
        for (var j = 0; j < usernameValueArray.length; j++) {
            if (!(inputValue.substring(0, inputValue.length) === usernameValueArray[j].substring(0, inputValue.length).toLowerCase())) {
                dropdownUsernameItems[j].classList.add('closed');
            } else {
                dropdownUsernameItems[j].classList.remove('closed');
            }
        }
    } else {
        for (var i = 0; i < dropdownUsernameItems.length; i++) {
            dropdownUsernameItems[i].classList.remove('closed');
        }
    }
});

dropdownUsernameItems.forEach(function (item) {
    item.addEventListener('click', function (evt) {
        inputUsernameField.value = item.textContent;
        dropdownUsernameItems.forEach(function (dropdownUsernames) {
            dropdownUsernames.classList.add('closed');
        });
    });
});

inputUsernameField.addEventListener('focus', function () {
    inputUsernameField.placeholder = 'Type username to filter';
    dropdownUsernames.classList.add('open');
    dropdownUsernameItems.forEach(function (dropdownUsernames) {
        dropdownUsernames.classList.remove('closed');
    });
});

inputUsernameField.addEventListener('blur', function () {
    inputUsernameField.placeholder = 'Select username';
    dropdownUsernames.classList.remove('open');
});

document.addEventListener('click', function (evt) {
    var isDropdown = dropdownUsernames.contains(evt.target);
    var isInput = inputUsernameField.contains(evt.target);
    if (!isDropdown && !isInput) {
        dropdownUsernames.classList.remove('open');
    }
});