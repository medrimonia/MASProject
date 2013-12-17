%.pdf: %.tex
	pdflatex $< && pdflatex $< && pdflatex $<

LATEX_SRC= introduction.tex           \
           architectureLogicielle.tex \
           comportements.tex          \
           simulationsGraphiques.tex  \
           illusionDeVie.tex          \
           conclusion.tex

all: report.pdf

report.pdf: $(LATEX_SRC)

clean:
	rm *.aux *.snm *.toc *.log