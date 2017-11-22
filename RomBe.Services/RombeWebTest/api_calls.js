
function DeleteUser(email) {
    performRequest({
        requestType: "Delete",
        requestUrl: "Maintenance/DeleteUser?email=" + email
    });
}
