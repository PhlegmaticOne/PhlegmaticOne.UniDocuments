﻿namespace UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;

public record PlagiarismSearchRequest(UniDocument Document, int NDocuments, int InferEpochs, string ModelName);