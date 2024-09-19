import { Outlet } from "react-router-dom";
import InitialNavbar from "./InitialNavbar";

function InitialNavbarWrapper() {
    return (
        <>
            <div>
                <InitialNavbar />
            </div>
            <div>
                <Outlet />
            </div>
        </>
  );
}

export default InitialNavbarWrapper;