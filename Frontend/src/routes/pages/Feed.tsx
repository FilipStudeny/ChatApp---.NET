import { Container } from "@mui/material";

import UserPanel from "../../components/UserPanel";
import React from "react";

export const Feed: React.FunctionComponent = () => (
	<Container sx={{ display: "flex", flexDirection: "row" }}>
		<UserPanel />
	</Container>
);

