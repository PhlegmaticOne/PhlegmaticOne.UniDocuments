$(() => {
    const localPath = document.location.pathname;
    const navigationLinks = document.querySelectorAll(".nav-link");

    if (localPath === "/") {
        makeActive(navigationLinks[0]);
        return;
    }

    const activeLink = Array.from(navigationLinks).filter(x => x.href.includes(localPath))[0];

    if (activeLink !== undefined) {
        makeActive(activeLink);
    }

    function makeActive(link) {
        link.classList.add("active");
    }
})