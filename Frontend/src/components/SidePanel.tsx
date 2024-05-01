import { Box, Paper } from "@mui/material";
import React from "react";

interface SidePanelProps {
	children: React.ReactNode;
}

const SidePanel: React.FC<SidePanelProps> = ({ children }) => (
	<Box sx={{ m: 0, p: 0, borderRadius: 0 }} height={1}>
		<Paper elevation={5} sx={{ height: 1, display: "flex", flexDirection: "column", borderRadius: 0 }}>
			{children}
		</Paper>
	</Box>
);

export default SidePanel;
