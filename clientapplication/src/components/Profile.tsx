import { NavLink } from "react-router-dom";
import { AuthorizedUser } from "./AuthProvider";

function Profile() {
  return (
      <><span>Hello <AuthorizedUser value="email" /> It's your profile info.</span>
          <NavLink to="/logout">
              <br />
              <span>Logout: <AuthorizedUser value="email" /></span>
              <br/>
          </NavLink>
      </>
  );
}

export default Profile;