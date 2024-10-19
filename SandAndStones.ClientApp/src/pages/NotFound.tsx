import { Link } from "react-router-dom";

function NotFound() {
    return (
      <div>
        <p>Page Not Found!</p>

        <p>Go to Homepage <Link to="/">Homepage</Link></p>
      </div>
  );
}

export default NotFound;