import { createRouter, createWebHistory } from "vue-router";
import { isAuthenticated, isAdmin, isGuest, canCreate } from "@/shared/auth";
import HomeView from "@/views/HomeView.vue";
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
//import ManageClaims from "@/components/users/ManageClaims.vue";
import ManageClaimsOp from "@/components/users/ManageClaimsOp.vue";

const routes = [
    {
        path: "/",
        name: "Home",
        redirect: "/files/1", // Redirige a la primera página de archivos
        component: HomeView
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
        component: AddFile
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
        component: AddProcedure
    },
    {
        path: "/users/:page",
        name: "Users",
        props: true,
        component: UsersView
    },
    {
        path: "/users/details/:id",
        name: "User Details",
        props: true,
        component: UserDetails
    },
    {
        path: "/users/details/:id/claims",
        name: "Manage Claims",
        props: true,
        component: ManageClaimsOp
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

router.beforeEach((to) => {
    // Check authentication - call functions properly
    if (!isAuthenticated() && to.name !== "Login" && to.name !== "Signup" && to.name !== "Forgot Password") {
        return { name: "Login" }
    }
    if (isAuthenticated() && (to.name === "Login" || to.name === "Signup" || to.name === "Forgot Password")) {
        return { name: "Home" }
    }

    // Admin-only routes
    const adminRoutes = ["Users", "User Details", "Manage Claims"];
    if (!isAdmin() && !isGuest() && adminRoutes.includes(to.name)) {
        return { name: "Home" };
    }

    // Permission-based route protection
    const createRoutes = ["Add File", "Add Procedure"];
    if (!canCreate() && createRoutes.includes(to.name)) {
        // Redirect to appropriate listing page based on route
        if (to.name === "Add File") {
            return { name: "Files", params: { page: 1 } };
        }
        if (to.name === "Add Procedure") {
            return { name: "Procedures", params: { page: 1 } };
        }
    }
});

router.afterEach((to) => {
    document.title = "FMS | " + to.name;
});

export default router;