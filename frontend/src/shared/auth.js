const setToken = (token) => localStorage.setItem("token", token);
const removeToken = () => localStorage.removeItem("token");

function getTokenPayload() {
    try {
        const token = localStorage.getItem("token");
        if (!token) return null;
        
        const parts = token.split('.');
        if (parts.length !== 3) return null;
        
        var base64Url = parts[1];
        var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));

        const payload = JSON.parse(jsonPayload);
        
        // Check if token is expired
        if (payload.exp && Date.now() >= payload.exp * 1000) {
            removeToken();
            return null;
        }

        return payload;
    } catch (error) {
        console.error('Error parsing token:', error);
        removeToken();
        return null;
    }
};

const isAuthenticated = () => {
    const payload = getTokenPayload();
    return payload?.role?.includes("user") ?? false;
};

const isAdmin = () => {
    const payload = getTokenPayload();
    return payload?.role?.includes("admin") ?? false;
};

const getPermissions = () => getTokenPayload()?.permissions;
const canCreate = () => {
    const permissions = getPermissions();
    if (!permissions) return false;

    return permissions.includes("create");
};

const canUpdate = () => {
    const permissions = getPermissions();
    if (!permissions) return false;

    return permissions.includes("update");
};

const canDelete = () => {
    const permissions = getPermissions();
    if (!permissions) return false;

    return permissions.includes("delete");
};

export {
    setToken,
    removeToken,
    isAuthenticated,
    isAdmin,
    canCreate,
    canUpdate,
    canDelete
};