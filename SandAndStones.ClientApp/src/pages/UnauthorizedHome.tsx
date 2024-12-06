import { Outlet } from "react-router-dom";

function UnauthotizedHome() {
    return (
        <div>
            <Outlet />
        </div>
    );
}

export default UnauthotizedHome;