import { createRouter, createWebHistory } from "vue-router";
//import HomeView from "@/views/HomeView.vue";
import FilesView from "@/components/files/FilesView.vue";
import ProceduresView from "@/components/procedures/ProceduresView.vue";
import UsersView from "@/components/users/UsersView.vue";
import AccountView from "@/views/AccountView.vue";
import Login from "@/components/LogIn.vue";
import FileDetails from "@/components/files/FileDetails.vue";
import ProcedureDetails from "@/components/procedures/ProcedureDetails.vue";
import UserDetails from "@/components/users/UserDetails.vue";
import AddFile from "@/components/files/AddFile.vue";
import AddProcedure from "@/components/procedures/AddProcedure.vue";
import ProceduresByFileId from "@/components/procedures/ProceduresByFileId.vue";
import SignUp from "@/components/SignUp.vue";
import ForgotPassword from "@/components/ForgotPassword.vue";
import PageNotFound from "@/components/PageNotFound.vue";
import ManageClaims from "@/components/users/ManageClaims.vue";

const routes = [
    {
        path: "/",
        name: "Home",
        redirect: "/files/1", // Redirige a la primera página de archivos
        //component: HomeView
    },
    {
        path: "/login",
        name: "Login",
        component: Login
    },
    {
        path: "/signup",
        name: "Signup",
        component: SignUp
    },
    {
        path: "/files/:page",
        name: "Files",
        props: true,
        component: FilesView
    },
    {
        path: "/files/details/:id",
        name: "File Details",
        props: true,
        component: FileDetails
    },
    {
        path: "/files/add",
        name: "Add File",
        component: AddFile,
        beforeEnter: (to, from, next) => {
            if (!canCreate()) {
                next({ name: "Files" });
            } else {
                next();
            }
        }
    },
    {
        path: "/procedures/:page",
        name: "Procedures",
        props: true,
        component: ProceduresView,
        children: [
            {
                path: "file/:id",
                component: ProceduresByFileId
            },
        ]
    },
    {
        path: "/procedures/details/:id",
        name: "Procedure Details",
        props: true,
        component: ProcedureDetails
    },
    {
        path: "/procedures/add",
        name: "Add Procedure",
        component: AddProcedure,
        beforeEnter: (to, from, next) => {
            if (!canCreate()) {
                next({ name: "Procedures" });
            } else {
                next();
            }
        }
    },
    {
        path: "/users/:page",
        name: "Users",
        props: true,
        component: UsersView,
        beforeEnter: (to, from, next) => {
            if (!isAdmin()) {
                next({ name: "Home" });
            } else {
                next();
            }
        }
    },
    {
        path: "/users/details/:id",
        name: "User Details",
        props: true,
        component: UserDetails,
        beforeEnter: (to, from, next) => { 
            if (!isAdmin()) {
                next({ name: "Home" });
            } else {
                next();
            }
        }
    },
    {
        path: "/users/claims/:id",
        name: "Manage Claims",
        props: true,
        component: ManageClaims,
        beforeEnter: (to, from, next) => { 
            if (!isAdmin()) {
                next({ name: "Home" });
            } else {
                next();
            }
        }
    },
    {
        path: "/account/forgot-password",
        name: "Forgot Password",
        component: ForgotPassword
    },
    {
        path: "/account",
        name: "Account",
        component: AccountView
    },
    {
        path: "/:pathMatch(.*)*",
        name: "Not Found",
        component: PageNotFound
    }
]

const router = createRouter({
    routes,
    history: createWebHistory()
});

const getToken = () => localStorage.getItem("token");
function getTokenPayload() {
    var base64Url = getToken().split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
};

const isAdmin = () => getTokenPayload().role === "admin";
const canCreate = () => {
    const permissions = getTokenPayload().permissions;
    if (!permissions) return false;

    return permissions.includes("create");
};

router.beforeEach((to) => {
    if (to.name !== "Login" && to.name !== "Signup" && to.name !== "Forgot Password" && !getToken()) {
        return { name: "Login" }
    }
    if (getToken && (to.name === "Login" || to.name === "Signup" || to.name === "Forgot Password")) {
        return { name: "Home" }
    }
});

router.afterEach((to) => {
    document.title = "FMS | " + to.name;
});

export default router;