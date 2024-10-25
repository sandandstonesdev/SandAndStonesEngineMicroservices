import { Link } from "react-router-dom";

function Profile() {
    return (
        <>
            <p>Hello! It's your profile info page.</p><br />
            <Link to="/logout">
                <span>Logout</span>
            </Link>
          </>
      );
}

export default Profile;