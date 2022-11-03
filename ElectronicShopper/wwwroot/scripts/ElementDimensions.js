export function getDimensions(element) {
    return JSON.stringify(document.getElementById(element).getBoundingClientRect());
}