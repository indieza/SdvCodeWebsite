'use strict';

let inputNewUserAction = document.querySelector('.clearUserActionValue');
let dropdownNewActions = document.querySelector('.clearActionsList');
let dropdownNewActionItems = Array.from(document.querySelectorAll('#addActions'));

var userActionsValueArray = [];
dropdownNewActionItems.forEach(function (item) {
    userActionsValueArray.push(item.textContent);
});

var closeDropdown = function closeDropdown() {
    dropdownNewActions.classList.remove('open');
};

inputNewUserAction.addEventListener('input', function () {
    dropdownNewActions.classList.add('open');
    var inputValue = inputNewUserAction.value.toLowerCase();
    var valueSubstring = undefined;
    if (inputValue.length > 0) {
        for (var j = 0; j < userActionsValueArray.length; j++) {
            if (!(inputValue.substring(0, inputValue.length) === userActionsValueArray[j].substring(0, inputValue.length).toLowerCase())) {
                dropdownNewActionItems[j].classList.add('closed');
            } else {
                dropdownNewActionItems[j].classList.remove('closed');
            }
        }
    } else {
        for (var i = 0; i < dropdownNewActionItems.length; i++) {
            dropdownNewActionItems[i].classList.remove('closed');
        }
    }
});

dropdownNewActionItems.forEach(function (item) {
    item.addEventListener('click', function (evt) {
        inputNewUserAction.value = item.textContent;
        dropdownNewActionItems.forEach(function (dropdown) {
            dropdown.classList.add('closed');
        });
    });
});

inputNewUserAction.addEventListener('focus', function () {
    inputNewUserAction.placeholder = 'Type to filter';
    dropdownNewActions.classList.add('open');
    dropdownNewActionItems.forEach(function (dropdown) {
        dropdown.classList.remove('closed');
    });
});

inputNewUserAction.addEventListener('blur', function () {
    inputNewUserAction.placeholder = 'Select user action';
    dropdownNewActions.classList.remove('open');
});

document.addEventListener('click', function (evt) {
    var isDropdown = dropdownNewActions.contains(evt.target);
    var isInput = inputNewUserAction.contains(evt.target);
    if (!isDropdown && !isInput) {
        dropdownNewActions.classList.remove('open');
    }
});