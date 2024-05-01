import { Grid } from "@mui/material";
import { grey } from "@mui/material/colors";
import React from "react";
import { Outlet } from "react-router-dom";

import FriendsPanel from "../components/FriendsPanel";
import UserPanel from "../components/UserPanel";

const Layout: React.FunctionComponent = () => (
	<Grid container spacing={0} height={1}>
		<Grid item xs={2} minWidth={200}>
			<UserPanel />
		</Grid>
		<Grid item xs bgcolor={grey[900]}>
			<Outlet />
		</Grid>
		<Grid item xs={2} minWidth={200}>
			<FriendsPanel />
		</Grid>
	</Grid>
);

export default Layout;
