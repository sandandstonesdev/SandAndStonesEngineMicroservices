import { RouterProvider, createBrowserRouter } from "react-router-dom";
import Login from "../pages/Login";
import About from "../pages/About";
import Register from "../pages/Register";
import Profile from "./Profile";
import Assets from "../pages/Assets";
import Textures from "../pages/Textures";
import Logout from "./Logout";
import { useAuth } from "../context/AuthProvider";
import ProtectedRoute from "./ProtectedRoute";
import Home from "../pages/Home";
import UnauthotizedHome from "../pages/UnauthotizedHome";
import { AuthData } from "../types/AuthData";

const Routes = () => {
    const { isAuthenticated } = useAuth() as AuthData;

    const publicRoutes = [
        {
            path: "/",
            element: <UnauthotizedHome/>,
            children:
                [
                    {
                        path: '/',
                        element: <Login />,
                    },
                    {
                        path: '/login',
                        element: <Login />,
                    },
                    {
                        path: '/register',
                        element: <Register />,
                    }
                ]
        }
    ];

    const authRoutes = [
        {
            path: "/",
            element: <ProtectedRoute><Home /></ProtectedRoute>,
            children: [
                {
                    path: '/',
                    element: <About />,
                },
                {
                    path: '/about',
                    element: <About />,
                },
                {
                    path: '/profile',
                    element: <Profile />,
                },
                {
                    path: '/assets',
                    element: <Assets />,
                },
                {
                    path: '/textures',
                    element: <Textures />,
                },
                {
                    path: '/logout',
                    element: <Logout />,
                }
            ]
        },
    ];

    const router = createBrowserRouter([
        ...(isAuthenticated ? authRoutes : publicRoutes)
    ]);

    return <RouterProvider router={router} />;
};

export default Routes;