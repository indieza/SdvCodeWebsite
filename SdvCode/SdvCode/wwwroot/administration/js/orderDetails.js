function viewOrder(id) {
    document.getElementById("orderDetails").innerHTML =
        document.getElementById(`${id}-orderDetails`).innerHTML;
}