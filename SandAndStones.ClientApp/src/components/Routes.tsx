import { RouterProvider, createBrowserRouter } from "react-router-dom";
import Login from "../pages/Login";
import About from "../pages/About";
import Register from "../pages/Register";
import Assets from "../pages/Assets";
import Textures from "../pages/Textures";
import Logout from "./Logout";
import ProtectedRoute from "./ProtectedRoute";
import Home from "../pages/Home";
import UnauthotizedHome from "../pages/UnauthorizedHome";
import Profile from "../pages/Profile";
import { useAppSelector } from "../redux/store/Store";

const Routes = () => {
    const isAuthenticated = useAppSelector(state => state.auth.isAuthenticated);
    
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