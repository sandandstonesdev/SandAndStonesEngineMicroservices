import { Outlet, useLocation } from "react-router-dom";
import Navbar from "../components/Navbar.tsx";
import { useDispatch } from "react-redux";
import { useEffect } from "react";
import { checkTokenValidity } from "../redux/store/AuthSlice.tsx";
import { AppDispatch } from "../redux/store/Store.tsx";

function Home() {
    const dispatch = useDispatch<AppDispatch>();
    const location = useLocation();

    useEffect(() => {
        dispatch(checkTokenValidity());
    }, [dispatch, location]);

    return (
        <div>
            <Navbar />
            <Outlet />
        </div>
    );
}

export default Home;