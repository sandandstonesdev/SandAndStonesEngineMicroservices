import { NavLink } from "react-router-dom"

function Navbar() {
    return (
        <nav className="nav">
            <NavLink to="/about">About</NavLink>
            <NavLink to="/assets">Assets</NavLink>
            <NavLink to="/textures">Textures</NavLink>
            <NavLink to="/profile">Profile</NavLink>
            <NavLink to="/events">Events</NavLink>
        </nav>
  );
}

export default Navbar;