document.addEventListener("DOMContentLoaded", function () {
    var viewAllButton = document.querySelector(".buttonView");
    var tabContent = document.querySelector(".tab-content");
    var isExpanded = false;

    function listAll() {
        fetch("/Career/ListAll")
            .then(response => response.text())
            .then(data => {
                tabContent.innerHTML = data;
            });
    }

    function listCareer() {
        fetch("/Career/ListCareer")
            .then(response => response.text())
            .then(data => {
                tabContent.innerHTML = data;
            });
    }

    function toggleList() {
        if (isExpanded) {
            listCareer();
            viewAllButton.textContent = "View all";
            isExpanded = false;
        } else {
            listAll();
            viewAllButton.textContent = "View less";
            isExpanded = true;
        }
    }

    viewAllButton.addEventListener("click", toggleList);
});