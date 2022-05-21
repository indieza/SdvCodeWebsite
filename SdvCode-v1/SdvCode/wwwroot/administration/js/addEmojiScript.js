function selectedImage(inputField) {
    let fullPath = inputField.value;

    if (fullPath) {
        const name = event.target.files[0].name;
        const lastDot = name.lastIndexOf('.');

        const fileName = name.substring(0, lastDot);

        document.getElementById("emojiName").value = fileName;
    }
}